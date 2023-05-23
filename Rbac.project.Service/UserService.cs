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
    public class UserService: IUserService
    {
        public readonly IUserRepoistory dal;
        public UserService(IUserRepoistory dal)
        {
            this.dal = dal;
        }
        public async Task<User> UserLog(string name, string pwd)
        {
            var user = dal.LogUser(name, pwd);
            return await user;
        }
    }
}
