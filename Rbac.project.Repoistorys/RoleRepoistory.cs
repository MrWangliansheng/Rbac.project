using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using Rbac.project.IRepoistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Rbac.project.Repoistorys
{
    public class RoleRepoistory: BaseRepoistory<Role>,IRoleRepoistory
    {
        private readonly RbacDbContext db;
        public RoleRepoistory(RbacDbContext db) : base(db)
        {
            this.db = db;
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
                var list = db.Set<Role>().Where(m =>m.RoleIsDelete.Equals(false)& m.RoleParentId.Equals(id)).ToList();
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
        public override async Task<Role> InsertAsync(Role role)
        {
            var list =await db.Set<Role>().Where(m=>m.RoleName.Equals(role.RoleName)&&m.RoleParentId.Equals(role.RoleParentId)).ToListAsync();
            if (list.Count>0)
            {
                role.RoleId = -1;
                return role;
            }
            else
            {
                await db.Set<Role>().AddAsync(role);
                await db.SaveChangesAsync();
                return role;
            }
        }
        /// <summary>
        /// 逻辑删除角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<Role> LogicDeleteAsync(int id)
        {
            try
            {
                var role =await db.Role.FindAsync(id);

                role.RoleIsDelete = true;
                db.Update(role);
                await db.SaveChangesAsync();
                return role;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 查询全部角色信息
        /// </summary>
        /// <returns></returns>
        public override async Task<List<Role>> GetALL()
        {
            var list = db.Role.Where(m => m.RoleIsDelete.Equals(false)).ToListAsync();
            return await list;
        }
    }
}
