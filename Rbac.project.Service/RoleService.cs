using Rbac.project.Domain;
using Rbac.project.IRepoistory;
using Rbac.project.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Service
{
    public class RoleService:BaseService<Role>,IRoleService
    {
        private readonly IRoleRepoistory dal;
        public RoleService(IRoleRepoistory dal):base(dal)
        {
            this.dal = dal;
        }

        
        
    }
}
