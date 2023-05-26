using Rbac.project.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IRepoistory
{
    public interface IPowerRepoistory:IBaseRepoistory<ResultDtoData>
    {
        /// <summary>
        /// 菜单级联选择器绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultDtoData GetPowerTree(int id = 0);
    }
}
