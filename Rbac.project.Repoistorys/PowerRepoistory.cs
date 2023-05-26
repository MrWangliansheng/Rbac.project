using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using Rbac.project.IRepoistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Repoistorys
{
    public class PowerRepoistory:BaseRepoistory<ResultDtoData>,IPowerRepoistory
    {
        private readonly RbacDbContext db;
        public PowerRepoistory(RbacDbContext db):base(db)
        {
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

    }
}
