﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rbac.project.IRepoistory
{
    public interface IBaseRepoistory<T> where T : class 
    {
        Task<List<T>> GetALL();

        Task<T> InsertAsync(T t);

        T Insert(T t);

        Task<T> FindAsync(int id);

        T Update(T t);

         Task<T> LogicDeleteAsync(int id);

        Task<List<T>> GetAll (Expression<Func<T, bool>> predicate);

    }
}
