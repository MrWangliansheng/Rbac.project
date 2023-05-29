using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using Rbac.project.Domain.Enum;
using Rbac.project.IRepoistory;
using Rbac.project.IRepoistory.LogOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Repoistorys
{
    public class PowerRepoistory:BaseRepoistory<ResultDtoData>,IPowerRepoistory
    {
        private readonly RbacDbContext db;
        private readonly ILogDataRepoistory logdata;
        public PowerRepoistory(RbacDbContext db,ILogDataRepoistory logdata):base(db)
        {
            this.logdata = logdata;
            this.db = db;
        }
        #region 菜单级联绑定
        /// <summary>
        /// 权限菜单级联绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultDtoData GetPowerTree(int id = 0)
        {
            try
            {
                var list = db.Set<Power>().Where(m => m.PowerParentId.Equals(id)).ToList();
                var treelist = new List<TreeDto>();
                foreach (var item in list)
                {
                    TreeDto tree = new TreeDto();
                    tree.value = item.PowerId;
                    tree.label = item.PowerName;
                    tree.children = GetTreeDto(item.PowerId).Count > 0 ? GetTreeDto(item.PowerId) : null;
                    treelist.Add(tree);
                }
                return new ResultDtoData { Result = Result.Success, Message = "菜单级联查询成功", Data = treelist };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<TreeDto> GetTreeDto(int id)
        {
            try
            {
                var list = db.Set<Power>().Where(m => m.PowerParentId.Equals(id)).ToList();
                var treelist = new List<TreeDto>();
                foreach (var item in list)
                {
                    TreeDto tree = new TreeDto();
                    tree.value = item.PowerId;
                    tree.label = item.PowerName;
                    tree.children = GetTreeDto(item.PowerId).Count > 0 ? GetTreeDto(item.PowerId) : null;
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

        #region 菜单表格树形显示

        public async Task<ResultDtoData> GetPowerTreeTableLevelone(int id = 0)
        {
            var list =await db.Power.Where(m => m.PowerIsDelete.Equals(false)&m.PowerParentId.Equals(id)).ToListAsync();
            foreach (var item in list)
            {
                item.children =await GetPowerTreeSublevel(item.PowerId);
            }

            return new ResultDtoData { Result=Result.Success,Message="数据查询成功",Data=list };
        }
        public async Task<List<Power>> GetPowerTreeSublevel(int id)
        {
            var list = await db.Power.Where(m => m.PowerIsDelete.Equals(false) & m.PowerParentId.Equals(id)).ToListAsync();
            foreach (var item in list)
            {
                item.children =await GetPowerTreeSublevel(item.PowerId);
            }
            return list;
        }
        /// <summary>
        /// 权限菜单级联选择绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ResultDtoData GetPowerTreeData(int id = 0)
        {
            try
            {
                var list = db.Power.Where(m => m.PowerParentId.Equals(id)).ToList();
                var treelist = new List<TreeDto>();
                foreach (var item in list)
                {
                    TreeDto tree = new TreeDto();
                    tree.value = item.PowerId;
                    tree.label = item.PowerName;
                    tree.children = GetPowerTreeDataSublevel(item.PowerId).Count > 0 ? GetPowerTreeDataSublevel(item.PowerId) : null;
                    treelist.Add(tree);
                }
                
                logdata.CreateLog("/PowerRepoistory/GetPowerTreeData", "查询权限菜单成功", "");
                return new ResultDtoData { Result = Result.Success, Message = "查询权限菜单成功", Data = treelist };
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public List<TreeDto> GetPowerTreeDataSublevel(int id)
        {
            var list = db.Power.Where(m => m.PowerParentId.Equals(id)).ToList();
            var treelist = new List<TreeDto>();
            foreach (var item in list)
            {
                TreeDto tree = new TreeDto();
                tree.value = item.PowerId;
                tree.label = item.PowerName;
                tree.children = GetPowerTreeDataSublevel(item.PowerId).Count > 0 ? GetPowerTreeDataSublevel(item.PowerId) : null;
                treelist.Add(tree);
            }
            return treelist;
        }
        #endregion
        #region 权限菜单枚举列表
        public ResultDtoData GetPowerEnum()
        {
            var list = new List<object>();
            foreach (var item in Enum.GetValues<PowerEnum>())
            {
                list.Add(new
                {
                    id = item,
                    name = item.ToString()
                });
            }
            return new ResultDtoData { Result=Result.Success,Message="权限菜单枚举列表查询成功",Data=list};
        }

        
        #endregion

    }
}
