using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using Rbac.project.IRepoistory;
using Rbac.project.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Repoistorys
{
    public class UserRepoistory : BaseRepoistory<User>, IUserRepoistory
    {
        private readonly RbacDbContext db;
        public UserRepoistory(RbacDbContext db) : base(db)
        {
            this.db=db;
        }

        public async Task<User> LogUser(string name)
        {
            var user =await db.User.Where(m => m.UserName.Equals(name)).FirstOrDefaultAsync();
           
            return  user;
        }
    }
}
