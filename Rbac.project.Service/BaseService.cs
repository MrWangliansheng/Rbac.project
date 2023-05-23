using Rbac.project.IService;
using System;

namespace Rbac.project.Service
{
    public class BaseService<T>:IBaseService<T> where T : class
    {
    }
}
