using Microsoft.EntityFrameworkCore;
using Rbac.project.IRepoistory;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rbac.project.Repoistorys
{
    public class BaseRepoistory<T> : IBaseRepoistory<T> where T : class
    {
        private readonly RbacDbContext db;

        public BaseRepoistory(RbacDbContext db)
        {
            this.db = db;
        }

        public async Task<T> Query(Expression<Func<T, bool>> predicate)
        {
            return await db.Set<T>().FirstOrDefaultAsync(predicate);
        }
    }
}
