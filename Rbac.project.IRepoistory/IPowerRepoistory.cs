using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rbac.project.IRepoistory
{
    public interface IPowerRepoistory : IBaseRepoistory<PowerData>
    {
        /// <summary>
        /// 菜单级联选择器绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultDtoData GetPowerTree(int id = 0);

        Task<ResultDtoData> GetPowerTreeTableLevelone(int id = 0);

        Task<List<Power>> GetPowerTreeSublevel(int id);

        ResultDtoData GetPowerEnum();

        ResultDtoData GetPowerTreeData(int id = 0);
    }
}
