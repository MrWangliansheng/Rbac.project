using Rbac.project.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IService
{
    public interface IUserService:IBaseService<User>
    {
        Task<User> UserLog(string name,string pwd);
    }
}
