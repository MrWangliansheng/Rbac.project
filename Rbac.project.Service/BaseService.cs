using AutoMapper;
using Rbac.project.IRepoistory;
using Rbac.project.IService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rbac.project.Service
{
    public class BaseService<T, T1> : IBaseService<T, T1> where T : class where T1 : class
    {
        public readonly IBaseRepoistory<T> Idal;
        private readonly IMapper mapper;
        public BaseService(IBaseRepoistory<T> Idal, IMapper mapper)
        {
            this.Idal = Idal;
            this.mapper = mapper;
        }

        public virtual async Task<T1> FindAsync(int id)
        {
            var t = await Idal.FindAsync(id);
            var t1 = mapper.Map<T1>(t);
            return t1;
        }

        public virtual async Task<List<T>> GetALL(Expression<Func<T, bool>> predicate)
        {
            var list = await Idal.GetALL();
            return list;
        }

        public virtual async Task<List<T1>> GetPage(Expression<Func<T, bool>> predicate)
        {
            var list = await Idal.GetAll(predicate);
            var list1= mapper.Map<List<T1>>(list);
            return list1;
        }

        public virtual int Insert(T t)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T1> InsertAsync(T t)
        {
            var i = await Idal.InsertAsync(t);
            var t1 = mapper.Map<T1>(i);
            return t1;
        }

        public virtual async Task<T1> LogicDeleteAsync(int id)
        {
            var t= await Idal.LogicDeleteAsync(id);
            var t1=mapper.Map<T1>(t);
            return t1;
        }

        public virtual T1 Update(T t)
        {
            var t1 = mapper.Map<T1>(Idal.Update(t));
            return t1;
        }
    }
}
