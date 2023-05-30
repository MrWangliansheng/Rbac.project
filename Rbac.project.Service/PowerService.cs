using AutoMapper;
using Rbac.project.Domain.DataDisplay;
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
    public class PowerService:BaseService<PowerData, ResultDtoData>,IPowerService
    {
        private readonly IPowerRepoistory dal;
        private readonly IMapper mapper;
        public PowerService(IPowerRepoistory dal, IMapper mapper) :base(dal,mapper) 
        {
            this.dal = dal;
            this.mapper = mapper;
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

        #region 权限菜单操作

        
        #endregion
    }
}
