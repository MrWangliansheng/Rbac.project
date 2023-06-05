using Rbac.project.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IRepoistory.Eextend
{
    public interface ILogDataRepoistory:IBaseRepoistory<LogData> 
    {
        void CreateLog(string name,string message,string oper);
    }
}
