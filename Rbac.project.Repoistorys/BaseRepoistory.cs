using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Rbac.project.IRepoistory;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetALL()
        {
            var list = await db.Set<T>().ToListAsync();
            return list;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T Insert(T t)
        {
            db.Set<T>().Add(t);
            db.SaveChanges();
            return t;
        }
        /// <summary>
        /// 异步添加数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> InsertAsync(T t)
        {
            await db.Set<T>().AddAsync(t);
            await db.SaveChangesAsync();
            return t;
        }

        public async Task<T> Query(Expression<Func<T, bool>> predicate)
        {
            return await db.Set<T>().FirstOrDefaultAsync(predicate);
        }
    }
}
