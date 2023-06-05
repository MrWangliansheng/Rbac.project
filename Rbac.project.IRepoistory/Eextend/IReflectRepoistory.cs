using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IRepoistory.Eextend
{
    public interface IReflectRepoistory<TEntity> where TEntity : class
    {
        void CreateAudit(TEntity entity,string name);
        void DeleteAudit(TEntity entity, string name);
        void UpdateAudit(TEntity entity, string name);
    }
}
