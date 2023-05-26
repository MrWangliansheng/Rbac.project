using Rbac.project.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rbac.project.IService
{
    public interface IBaseService<T> where T : class
    {
        Task<List<T>> GetALL();

        Task<T> InsertAsync(T t);

        int Insert(T t);

        Task<T> FindAsync(int id);


        T Update(T t);

        Task<T> LogicDeleteAsync(int id);
    }
}
