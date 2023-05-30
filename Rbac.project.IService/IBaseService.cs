using Rbac.project.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rbac.project.IService
{
    public interface IBaseService<T,T1> where T : class where T1 : class
    {
        Task<List<T>> GetALL(Expression<Func<T, bool>> predicate);

        Task<T1> InsertAsync(T t);

        int Insert(T t);

        Task<T1> FindAsync(int id);


        T1 Update(T t);

        Task<T1> LogicDeleteAsync(int id);

        Task<List<T1>> GetPage(Expression<Func<T, bool>> predicate);
    }
}
