using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IService
{
    public interface IRoleService:IBaseService<Role>
    {
        ResultDtoData GetRoleTree();
    }
}
