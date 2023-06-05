using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IRepoistory
{
    public interface IRoleRepoistory:IBaseRepoistory<RoleData>
    {
        ResultDtoData GetRoleTree(int id=0);

        ResultDto GetRoleName(int id,string name);
    }
}
