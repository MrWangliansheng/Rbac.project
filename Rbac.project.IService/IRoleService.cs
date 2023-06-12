using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using System.Threading.Tasks;

namespace Rbac.project.IService
{
    public interface IRoleService : IBaseService<RoleData, ResultDtoData>
    {
        ResultDtoData GetRoleTree();

        Task<PageDto> GetRolePage(RoleDto dto);

        ResultDtoData GetRolePowerButton(int id,int state);

        ResultDtoData GetRolePower();
    }
}
