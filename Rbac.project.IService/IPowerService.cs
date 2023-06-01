using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IService
{
    public interface IPowerService:IBaseService<PowerData, ResultDtoData>
    {
        /// <summary>
        /// 菜单级联选择器绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultDtoData GetPowerTree();

        Task<ResultDtoData> GetPowerTreeTableLevelone(int id = 0);

        ResultDtoData GetPowerEnum();
        ResultDtoData GetPowerTreeData();

        Task<ResultDtoData> GetPar(int id);
    }
}
