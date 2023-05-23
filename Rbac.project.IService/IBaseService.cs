using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rbac.project.IService
{
    public interface IBaseService<T>where T : class
    {
        Task<List<T>> GetALL();

        Task<int> InsertAsync(T t);

        int Insert(T t);

    }
}
