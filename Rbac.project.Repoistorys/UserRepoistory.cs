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

namespace Rbac.project.Repoistorys
{
    public class UserRepoistory : BaseRepoistory<UserData>, IUserRepoistory
    {
        private readonly RbacDbContext db;
        private readonly IMapper mapper;
        public UserRepoistory(RbacDbContext db, IMapper mapper) : base(db)
        {
            this.mapper = mapper;
            this.db = db;
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
            var user = db.Set<User>().Find(userid);
            user.UserPassword = password.Md5();
            var i = db.SaveChanges();
            return i;
        }

        public override async Task<UserData> InsertAsync(UserData User)
        {
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
                    var user = mapper.Map<User>(User);
                    await db.AddAsync(user);
                    await db.SaveChangesAsync();
                    foreach (var item in User.RoleId)
                    {
                        var userrole = new UserRole();
                        userrole.RoleID = item;
                        userrole.UserID = user.UserId;
                        await db.AddAsync(userrole);
                        await db.SaveChangesAsync();
                    }
                    return mapper.Map<UserData>(user);
                }
            }
            catch (Exception)
            {

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
            var user = await db.User.FindAsync(id);
            user.UserIsDelete = true;
            await db.SaveChangesAsync();
            return mapper.Map<UserData>(user);
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
                userdata.RoleId = roleid;
                return userdata;
            }
            catch (Exception ex)
            {
                var log = new LogData { LogName= "/UserRepoistory/FindAsync",LogMessage=ex.Message,Operator="" };
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
            try
            {
                var use = db.User.Where(m => m.UserId.Equals(user.UserId) & m.UserName.Equals(user.UserName)).ToList().FirstOrDefault();
                if (use != null)
                {
                    var data = mapper.Map(user, use);
                    db.Update(data);
                    var i = db.SaveChanges();
                    if (i > 0)
                    {
                        var userrolelist = db.UserRole.Where(m => m.UserID.Equals(user.UserId)).ToList();
                        db.RemoveRange(userrolelist);
                        if (user.RoleId!=null)
                        {
                            foreach (var item in user.RoleId)
                            {
                                var userrole = new UserRole();
                                userrole.RoleID = item;
                                userrole.UserID = data.UserId;
                                userrolelist.Add(userrole);
                            }
                            db.AddRange(userrolelist);
                            db.SaveChanges();
                        }
                        #region 添加日志信息
                        var log = new LogData { LogName = "/UserRepoistory/Update", LogMessage = "修改了用户信息userid:" + user.UserId, Operator = "" };
                        #endregion
                        transaction.Commit();
                        return user;
                    }
                    else
                    {
                        user.UserId = -2;//表示修改未成功
                        return user;
                    }
                }
                else
                {
                    use = db.User.Where(m => m.UserName.Equals(user.UserName)).ToList().FirstOrDefault();
                    if (use != null)
                    {
                        user.UserId = -1;
                        return user;
                    }
                    var data = mapper.Map(user, use);
                    db.Update(data);
                    var i = db.SaveChanges();
                    var log = new LogData { LogName = "/UserRepoistory/Update", LogMessage = "修改了用户信息userid:" + user.UserId, Operator = "" };
                    transaction.Commit();
                    if (i > 0)
                    {
                        return user;
                    }
                    else
                    {
                        user.UserId = -2;//表示修改未成功
                        return user;
                    }
                }
                
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var log = new LogData { LogName = "/UserRepoistory/Update", LogMessage = ex.Message, Operator = "" };
                user.UserId = -1;
                return user;
            }

        }
    }
}
