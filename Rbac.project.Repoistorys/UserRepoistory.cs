using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using Rbac.project.IRepoistory;
using Rbac.project.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rbac.project.Utility;
using System.Security.Cryptography;
using Rbac.project.Domain.Dto;
using AutoMapper;

using Microsoft.Extensions.DependencyInjection;
using Rbac.project.Repoistorys.AutoMapper;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.IRepoistory.Eextend;
using Rbac.project.Domain.ParentIdAll;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;

namespace Rbac.project.Repoistorys
{
    public class UserRepoistory : BaseRepoistory<UserData>, IUserRepoistory
    {
        private readonly RbacDbContext db;
        private readonly IMapper mapper;
        private readonly ILogDataRepoistory logdata;
        private readonly IHttpContextAccessor http;
        private readonly IReflectRepoistory<User> reflect;
        public UserRepoistory(RbacDbContext db, IMapper mapper, ILogDataRepoistory logdata, IHttpContextAccessor http, IReflectRepoistory<User> reflect) : base(db)
        {
            this.mapper = mapper;
            this.db = db;
            this.logdata = logdata;
            this.http = http;
            this.reflect = reflect;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<User> LogUser(string name)
        {
            var user = await db.User.Where(m => m.UserName.Equals(name)).FirstOrDefaultAsync();

            return user;
        }
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int ResetUserPasswrod(int userid, string password)
        {
            var tran = db.Database.BeginTransaction();
            var name = http.HttpContext.User.Claims.Where(m => m.Type == "name").FirstOrDefault().Value;
            try
            {
                var user = db.Set<User>().Find(userid);
                user.UserPassword = password.Md5();
                var i = db.SaveChanges();
                logdata.CreateLog("/UserRepoistory/ResetUserPasswrod", "重置密码", name);
                tran.Commit();
                return i;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                logdata.CreateLog("/UserRepoistory/ResetUserPasswrod", ex.Message, name);
                return -1;

            }

        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public override async Task<UserData> InsertAsync(UserData User)
        {
            var tran = db.Database.BeginTransaction();
            var name = http.HttpContext.User.Claims.Where(m => m.Type == "name").FirstOrDefault().Value;
            try
            {
                var list = await db.Set<User>().Where(m => m.UserName.Equals(User.UserName)).ToListAsync();
                if (list.Count > 0)
                {
                    User.UserId = -1;
                    return User;
                }
                else
                {
                    //数据映射
                    var user = mapper.Map<User>(User);
                    user.UserPassword=user.UserPassword.Md5();
                    await db.AddAsync(user);//上下文异步添加用户信息
                    await db.SaveChangesAsync();//保存更改
                    //添加用户角色中间表数据
                    foreach (var item in User.RoleId)
                    {
                        var userrole = new UserRole();
                        userrole.RoleID = item;
                        userrole.UserID = user.UserId;
                        await db.AddAsync(userrole);
                    }
                    //添加用户角色全部ID表数据用于反填
                    foreach (var item in User.RoleIdAll)
                    {
                        var uria = new UserRoleIdAll();
                        uria.UserId = user.UserId;
                        uria.RoleIdAll = item;
                        await db.AddAsync(uria);
                    }
                    await db.SaveChangesAsync();
                    //修改审计字段值
                    reflect.CreateAudit(user, name);
                    //添加日志表数据
                    logdata.CreateLog("/UserRepoistory/InsertAsync", "添加用户信息", " ");
                    //提交事务
                    tran.Commit();
                    return mapper.Map<UserData>(user);
                }
            }
            catch (Exception ex)
            {
                //出现异常后事务回滚，不做任何更改
                tran.Rollback();
                //添加错误信息到日志表数据
                logdata.CreateLog("/UserRepoistory/InsertAsync", ex.Message, "");
                return null;
            }

        }
        /// <summary>
        /// 分页查询用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PageDto> GetUserInfoPage(UserDto dto)
        {
            var list = db.Set<User>().Where(m => m.UserIsDelete.Equals(false)).AsQueryable();

            if (!string.IsNullOrEmpty(dto.name))
            {
                list = list.Where(m => m.PullName.Contains(dto.name) || m.UserName.Contains(dto.name));
            }
            if (dto.createstartime != null)
            {
                list = list.Where(m => m.UserCreateTime >= dto.createstartime);
            }
            if (dto.createendtime != null)
            {
                list = list.Where(m => m.UserCreateTime <= dto.createendtime);
            }

            dto.total = list.Count();
            dto.pagecount = (int)Math.Ceiling(dto.total * 1.0 / dto.pagesize);

            list = list.Skip((dto.pageindex - 1) * dto.pagesize).Take(dto.pagesize);

            return new PageDto { Result = Result.Success, Message = "用户分页信息查询成功", Data = mapper.Map<List<UserData>>(await list.ToListAsync()), total = dto.total, pagecount = dto.pagecount };
        }
        /// <summary>
        /// 逻辑删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<UserData> LogicDeleteAsync(int id)
        {
            var name = http.HttpContext.User.Claims.Where(m => m.Type == "name").FirstOrDefault().Value;

            var tran = db.Database.BeginTransaction();
            try
            {
                var user = await db.User.FindAsync(id);
                user.UserIsDelete = true;
                await db.SaveChangesAsync();
                reflect.DeleteAudit(user, name);
                logdata.CreateLog("/UserRepoistory/LogicDeleteAsync", "删除用户信息", "");
                tran.Commit();
                return mapper.Map<UserData>(user);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                logdata.CreateLog("/UserRepoistory/LogicDeleteAsync", ex.Message, "");
                return null;
            }

        }
        /// <summary>
        /// 重写用户回写方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<UserData> FindAsync(int id)
        {
            try
            {
                var user = await db.User.FindAsync(id);
                var userdata = mapper.Map<UserData>(user);
                var userrolelist = db.UserRole.Where(m => m.UserID.Equals(id)).ToList();
                List<int> roleid = new List<int>();
                foreach (var item in userrolelist)
                {
                    roleid.Add(item.RoleID);
                }
                var userroleidall = db.UserRoleIdAll.Where(m => m.UserId.Equals(id)).ToList();
                List<string> roleidall = new List<string>();
                foreach (var item in userroleidall)
                {
                    roleidall.Add(item.RoleIdAll);
                }
                userdata.RoleId = roleid;
                userdata.RoleIdAll = roleidall;
                return userdata;
            }
            catch (Exception ex)
            {
                var log = new LogData { LogName = "/UserRepoistory/FindAsync", LogMessage = ex.Message, Operator = "" };
                db.Add(log);
                return null;
            }

        }
        /// <summary>
        /// 重写修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override UserData Update(UserData user)
        {
            var transaction = db.Database.BeginTransaction();
            var name = "";
            if (http.HttpContext.User.Claims.Count() > 0)
            {
                 name=http.HttpContext.User.Claims.Where(m => m.Type == "name").FirstOrDefault().Value ;
            }

            try
            {
                #region 修改用户角色中间表数据
                if (user.RoleId != null)
                {
                    var userrolelist = db.UserRole.Where(m => m.UserID.Equals(user.UserId)).ToList();
                    var userroleidall = db.UserRoleIdAll.Where(m => m.UserId.Equals(user.UserId)).ToList();
                    var uslist = new List<UserRole>();
                    db.RemoveRange(userrolelist);
                    db.RemoveRange(userroleidall);
                    foreach (var item in user.RoleId)
                    {
                        var userrole = new UserRole();
                        userrole.RoleID = item;
                        userrole.UserID = user.UserId;
                        db.Add(userrole);
                    }
                    foreach (var item in user.RoleIdAll)
                    {
                        var userroleall = new UserRoleIdAll();
                        userroleall.UserId = user.UserId;
                        userroleall.RoleIdAll = item;
                        db.Add(userroleall);
                    }
                    db.SaveChanges();
                }
                #endregion

                var use = db.User.Where(m => m.UserId.Equals(user.UserId) & m.UserName.Equals(user.UserName)).ToList().FirstOrDefault();

                if (use != null)
                {
                    var data = mapper.Map(user, use);
                    db.Update(data);
                    var i = db.SaveChanges();
                    if (i > 0)
                    {

                        #region 添加日志信息
                        //var log = new LogData { LogName = "/UserRepoistory/Update", LogMessage = "修改了用户信息userid:" + user.UserId, Operator = "" };
                        if (!string.IsNullOrEmpty(name))
                        {
                            logdata.CreateLog("/UserRepoistory/Update", "修改了用户信息", name);
                            //修改审计字段
                            reflect.UpdateAudit(data, name);
                        }
                        #endregion

                        transaction.Commit();
                        return user;
                    }
                    else
                    {

                        transaction.Rollback();
                        user.UserId = -2;//表示修改未成功
                        return user;
                    }
                }
                else
                {
                    use = db.User.Where(m => m.UserName.Equals(user.UserName)).ToList().FirstOrDefault();
                    if (use != null)
                    {
                        user.UserId = -1;//用户名已存在
                        return user;
                    }
                    var data = mapper.Map(user, use);
                    db.Update(data);
                    var i = db.SaveChanges();
                    if (!string.IsNullOrEmpty(name))
                    {
                        logdata.CreateLog("/UserRepoistory/Update", "修改了用户信息", name);
                        //修改审计字段
                        reflect.UpdateAudit(data, name);
                    }
                    transaction.Commit();
                    if (i > 0)
                    {
                        return user;
                    }
                    else
                    {
                        transaction.Rollback();
                        user.UserId = -2;//表示修改未成功
                        return user;
                    }
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var log = new LogData { LogName = "/UserRepoistory/Update", LogMessage = ex.Message, Operator = "" };
                logdata.CreateLog("/UserRepoistory/Update", ex.Message, name);
                //user.UserId = -1;
                return null;//出现异常
            }
        }
    }
}
