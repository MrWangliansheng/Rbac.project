using Rbac.project.Domain.Dto;
using Rbac.project.IRepoistory;
using Rbac.project.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Service
{
    public class PowerService:BaseService<ResultDtoData>,IPowerService
    {
        private readonly IPowerRepoistory dal;
        public PowerService(IPowerRepoistory dal):base(dal) 
        {
            this.dal = dal;
        }

        public ResultDtoData GetPowerEnum()
        {
            return dal.GetPowerEnum();
        }

        /// <summary>
        /// 菜单级联选择器绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultDtoData GetPowerTree()
        {
            return dal.GetPowerTree();
        }
        /// <summary>
        /// 菜单权限级联绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ResultDtoData GetPowerTreeData()
        {
            return dal.GetPowerTreeData();
        }

        /// <summary>
        /// 树形菜单列表显示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultDtoData> GetPowerTreeTableLevelone(int id = 0)
        {
            return await dal.GetPowerTreeTableLevelone(id);
        }
    }
}
