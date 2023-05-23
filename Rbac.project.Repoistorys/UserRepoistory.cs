using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using Rbac.project.IRepoistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Repoistorys
{
    public class UserRepoistory : IUserRepoistory
    {
        public readonly RbacDbContext db;
        public UserRepoistory(RbacDbContext db)
        {
            this.db = db;
        }
        public async Task<User> LogUser(string name, string pwd)
        {
            var user = db.User.Where(m => m.UserName.Equals(name) & m.UserPwaword.Equals(pwd)).FirstOrDefaultAsync();
            return await user;
        }
    }
}
