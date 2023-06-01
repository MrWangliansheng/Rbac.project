using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using Rbac.project.Domain.ParentIdAll;
using Rbac.project.IRepoistory;
using Rbac.project.IRepoistory.LogOperation;
using Rbac.project.Repoistory.LogOperation;
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
        public RoleRepoistory(RbacDbContext db, ILogDataRepoistory logdata, IMapper mapper) : base(db)
        {
            this.db = db;
            this.logdata = logdata;
            this.mapper = mapper;
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
                    logdata.CreateLog("/RoleRepoistory/InsertAsync", "添加角色", "");
                    tran.Commit();
                    return mapper.Map<RoleData>(ro);
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                logdata.CreateLog("/RoleRepoistory/InsertAsync", ex.Message, "");
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
            try
            {
                var role = await db.Role.FindAsync(id);

                role.RoleIsDelete = true;
                db.Update(role);
                logdata.CreateLog("/RoleRepoistory/LogicDeleteAsync", "删除角色信息成功", "");
                await db.SaveChangesAsync();
                await tran.CommitAsync();
                return mapper.Map<RoleData>(role);
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                logdata.CreateLog("/RoleRepoistory/LogicDeleteAsync", "删除角色异常:" + ex.Message, "");
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
    }
}
