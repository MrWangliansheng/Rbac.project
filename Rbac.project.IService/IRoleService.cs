using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IService
{
    public interface IRoleService : IBaseService<RoleData, ResultDtoData>
    {
        ResultDtoData GetRoleTree();

        Task<PageDto> GetRolePage(RoleDto dto);

        ResultDto GetRoleName(int id, string name);
    }
}
