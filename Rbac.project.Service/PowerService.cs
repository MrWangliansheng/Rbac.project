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
        /// <summary>
        /// 菜单级联选择器绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultDtoData GetPowerTree()
        {
            return dal.GetPowerTree();
        }
    }
}
