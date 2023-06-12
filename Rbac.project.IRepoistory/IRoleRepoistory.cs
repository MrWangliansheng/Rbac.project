using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;

namespace Rbac.project.IRepoistory
{
    public interface IRoleRepoistory:IBaseRepoistory<RoleData>
    {
        ResultDtoData GetRoleTree(int id=0);

        ResultDtoData GetRolePowerButton(int id,int state);
        ResultDtoData GetRolePower();
    }
}
