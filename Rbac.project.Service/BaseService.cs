﻿using Rbac.project.IRepoistory;
using Rbac.project.IService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rbac.project.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        public readonly IBaseRepoistory<T> Idal;
        public BaseService(IBaseRepoistory<T> Idal)
        {
            this.Idal = Idal;
        }
        public Task<List<T>> GetALL()
        {
            var list =Idal.GetALL();
            return list;
        }

        public int Insert(T t)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(T t)
        {
            throw new NotImplementedException();
        }
    }
}
