﻿using Microsoft.EntityFrameworkCore;
using Rbac.project.IRepoistory;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public virtual async Task<T> FindAsync(int id)
        {
            return await db.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetALL()
        {
            var list = await db.Set<T>().ToListAsync();
            return list;
        }
        /// <summary>
        /// 查询分页信息
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            var list=await db.Set<T>().Where(predicate).ToListAsync();
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
        public virtual async Task<T> InsertAsync(T t)
        {
            await db.Set<T>().AddAsync(t);
            await db.SaveChangesAsync();
            return t;
        }
        /// <summary>
        /// 逻辑删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> LogicDeleteAsync(int id)
        {
            var t= await db.Set<T>().FindAsync(id);
            db.Set<T>().Update(t);
            return t;
        }

        public async Task<T> Query(Expression<Func<T, bool>> predicate)
        {
            return await db.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual T Update(T t)
        {
            db.Set<T>().Update(t);
            db.SaveChanges();
            return  t;
        }
    }
}
