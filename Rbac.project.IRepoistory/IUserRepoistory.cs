using Rbac.project.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IRepoistory
{
    public interface IUserRepoistory:IBaseRepoistory<User>
    {
        Task<User> LogUser(string name);
    }
}
