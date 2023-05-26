using Rbac.project.IRepoistory;
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

        public async Task<T> FindAsync(int id)
        {
           return await Idal.FindAsync(id);
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

        public virtual async Task<T> InsertAsync(T t)
        {
            return await Idal.InsertAsync(t);
        }

        public async Task<T> LogicDeleteAsync(int id)
        {
            return await Idal.LogicDeleteAsync(id);
        }

        public T Update(T t)
        {
            return Idal.Update(t);
        }
    }
}
