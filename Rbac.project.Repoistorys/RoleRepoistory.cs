using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using Rbac.project.Domain.ParentIdAll;
using Rbac.project.IRepoistory;
using Rbac.project.IRepoistory.Eextend;
using Rbac.project.Repoistory.Eextend;
using Rbac.project.Repoistorys.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Rbac.project.Repoistorys
{
    public class RoleRepoistory : BaseRepoistory<RoleData>, IRoleRepoistory
    {
        private readonly RbacDbContext db;
        private readonly ILogDataRepoistory logdata;
        private readonly IMapper mapper;
        private readonly IReflectRepoistory<Role> reflect;
        private readonly IHttpContextAccessor http;
        public RoleRepoistory(RbacDbContext db, ILogDataRepoistory logdata, IMapper mapper, IReflectRepoistory<Role> reflect,IHttpContextAccessor http) : base(db)
        {
            this.db = db;
            this.logdata = logdata;
            this.mapper = mapper;
            this.reflect = reflect;
            this.http = http;
        }
        #region 角色级联选择器绑定
        /// <summary>
        /// 角色级联选择
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultDtoData GetRoleTree(int id = 0)
        {
            try
            {
                var list = db.Set<Role>().Where(m => m.RoleIsDelete.Equals(false) & m.RoleParentId.Equals(id)).ToList();
                var treelist = new List<TreeDto>();
                foreach (var item in list)
                {
                    TreeDto tree = new TreeDto();
                    tree.value = item.RoleId;
                    tree.label = item.RoleName;
                    tree.children = GetTreeDto(item.RoleId).Count > 0 ? GetTreeDto(item.RoleId) : null;
                    treelist.Add(tree);
                }
                var res = new ResultDtoData { Result = Result.Success, Message = "树形结构查询成功", Data = treelist };
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 查询树数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<TreeDto> GetTreeDto(int id)
        {
            try
            {
                var list = db.Set<Role>().Where(m => m.RoleIsDelete.Equals(false) & m.RoleParentId.Equals(id)).ToList();
                var treelist = new List<TreeDto>();
                foreach (var item in list)
                {
                    TreeDto tree = new TreeDto();
                    tree.value = item.RoleId;
                    tree.label = item.RoleName;
                    tree.children = GetTreeDto(item.RoleId).Count > 0 ? GetTreeDto(item.RoleId) : null;
                    treelist.Add(tree);
                }
                return treelist;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        /// <summary>
        /// 异步添加数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<RoleData> InsertAsync(RoleData role)
        {
            var tran = db.Database.BeginTransaction();
            string name = http.HttpContext.User.Claims.Where(m => m.Type == "name").FirstOrDefault().Value;
            try
            {
                var list = await db.Set<Role>().Where(m => m.RoleName.Equals(role.RoleName) && m.RoleParentId.Equals(role.RoleParentId)).ToListAsync();
                if (list.Count > 0)
                {
                    role.RoleId = -1;
                    return role;
                }
                else
                {
                    var ro = mapper.Map<Role>(role);
                    await db.Set<Role>().AddAsync(ro);
                    await db.SaveChangesAsync();
                    foreach (var item in role.PowerId)
                    {
                        var rolepower = new RolePower();
                        rolepower.PowerID = item;
                        rolepower.RoleID = ro.RoleId;
                        await db.AddAsync(rolepower);
                    }
                    foreach (var item in role.PowerIdAll)
                    {
                        var idall = new RolePowerIdAll();
                        idall.PowerIdAll = item;
                        idall.RoleID = ro.RoleId;
                        await db.AddAsync(idall);
                    }
                    await db.SaveChangesAsync();
                    logdata.CreateLog("/RoleRepoistory/InsertAsync", "添加角色", name);
                    reflect.CreateAudit(ro, name);
                    tran.Commit();
                    return mapper.Map<RoleData>(ro);
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                logdata.CreateLog("/RoleRepoistory/InsertAsync", ex.Message, name);
                return null;
            }

        }
        /// <summary>
        /// 逻辑删除角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<RoleData> LogicDeleteAsync(int id)
        {
            var tran = await db.Database.BeginTransactionAsync();
            string name = http.HttpContext.User.Claims.Where(m => m.Type == "name").FirstOrDefault().Value;
            try
            {
                var role = await db.Role.FindAsync(id);

                role.RoleIsDelete = true;
                db.Update(role);
                logdata.CreateLog("/RoleRepoistory/LogicDeleteAsync", "删除角色信息成功", name);
                await db.SaveChangesAsync();
                reflect.DeleteAudit(role, name);
                await tran.CommitAsync();
                return mapper.Map<RoleData>(role);
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                logdata.CreateLog("/RoleRepoistory/LogicDeleteAsync", "删除角色异常:" + ex.Message, name);
                return null;
            }
        }

        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override async Task<List<RoleData>> GetAll(Expression<Func<RoleData, bool>> predicate)
        {
            var role = mapper.Map<List<RoleData>>(await db.Role.ToListAsync());
            return role.AsQueryable().Where(predicate).ToList();
        }
        /// <summary>
        /// 角色信息反填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<RoleData> FindAsync(int id)
        {
            string name = http.HttpContext.User.Claims.Where(m => m.Type == "name").FirstOrDefault().Value;
            try
            {
                var role = await db.Role.FindAsync(id);
                List<int> paridall=new List<int>();
                var paridlist=db.RolePower.Where(m=>m.RoleID==id).ToList();
                foreach (var item in paridlist)
                {
                    paridall.Add(item.PowerID);
                }
                var idalllist=db.RolePowerIdAll.Where(m=>m.RoleID == id).ToList();
                List<string> powidall = new List<string>();
                foreach (var item in idalllist)
                {
                    powidall.Add(item.PowerIdAll);
                }
                var data= mapper.Map<RoleData>(role);
                data.PowerIdAll = powidall;
                return data;
            }
            catch (Exception ex)
            {
                logdata.CreateLog("/RoleRepoistory/FindAsync", ex.Message, name);
                return null;
            }
           
        }
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override RoleData Update(RoleData t)
        {
            var tran=db.Database.BeginTransaction();
            string name = http.HttpContext.User.Claims.Where(m => m.Type == "name").FirstOrDefault().Value;
            try
            {
                var role = db.Role.Where(m => m.RoleId.Equals(t.RoleId) && m.RoleName.Equals(t.RoleName)).FirstOrDefault();
                if (role != null)
                {
                    mapper.Map(t,role);
                    db.Update(role);
                    db.SaveChanges();
                    var powerlist = db.RolePower.Where(m => m.RoleID.Equals(t.RoleId)).ToList();
                    var poweridall = db.RolePowerIdAll.Where(m => m.RoleID.Equals(t.RoleId)).ToList();
                    db.RemoveRange(powerlist);
                    db.RemoveRange( poweridall);

                    foreach (var item in t.PowerIdAll)
                    {
                        var rp = new RolePowerIdAll();
                        rp.RoleID = role.RoleId;
                        rp.PowerIdAll = item;
                        db.Add(rp);
                    }
                    foreach (var item in t.PowerId)
                    {
                        var rolepower = new RolePower();
                        rolepower.RoleID = role.RoleId;
                        rolepower.PowerID = item;
                        db.Add(rolepower);
                    }
                    db.SaveChanges();
                    logdata.CreateLog("/RoleRepoistory/Update", "修改角色信息", name);
                    reflect.UpdateAudit(role, name);
                    tran.Commit();
                    return mapper.Map(role, t);
                }
                else
                {
                    role = db.Role.Where(m=>m.RoleParentId.Equals(t.RoleParentId)&&m.RoleName.Equals(t.RoleName)).FirstOrDefault();
                    if (role!=null)
                    {
                        role.RoleId = -1;
                        return mapper.Map(role, t);
                    }
                    else
                    {
                        mapper.Map(t, role);
                        db.Update(role);
                        db.SaveChanges();
                        var powerlist = db.RolePower.Where(m => m.RoleID.Equals(t.RoleId)).ToList();
                        var poweridall = db.RolePowerIdAll.Where(m => m.RoleID.Equals(t.RoleId)).ToList();
                        db.RemoveRange(powerlist);
                        db.RemoveRange(poweridall);

                        foreach (var item in t.PowerIdAll)
                        {
                            var rp = new RolePowerIdAll();
                            rp.RoleID = role.RoleId;
                            rp.PowerIdAll = item;
                            db.Add(rp);
                        }
                        foreach (var item in t.PowerId)
                        {
                            var rolepower = new RolePower();
                            rolepower.RoleID = role.RoleId;
                            rolepower.PowerID = item;
                            db.Add(rolepower);
                        }
                        db.SaveChanges();
                        logdata.CreateLog("/RoleRepoistory/Update", "修改角色信息", name);
                        reflect.UpdateAudit(role, name);
                        tran.Commit();
                        return mapper.Map(role, t);
                    }
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                logdata.CreateLog("/RoleRepoistory/Update", ex.Message, name);
                return null;
            }
        }
        /// <summary>
        /// 查询角色名称是否重复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultDto GetRoleName(int id, string name)
        {
            var role = db.Role.Where(m => m.RoleParentId.Equals(id) && m.RoleName.Equals(name)).ToList().FirstOrDefault();
            if (role!=null)
            {
                return new ResultDto { Result = Result.Success };
            }
            else
            {
                role = db.Role.Where(m => m.RoleName.Equals(name)).FirstOrDefault();
                if (role!=null)
                {
                    return new ResultDto { Result = Result.Warning ,Message="角色已存在"};
                }
                else
                {
                    return new ResultDto { Result = Result.Success };
                }
            }
        }
    }
}
