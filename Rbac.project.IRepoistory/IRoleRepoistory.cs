using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IRepoistory
{
    public interface IRoleRepoistory:IBaseRepoistory<Role>
    {
        ResultDtoData GetRoleTree(int id=0);


    }
}
