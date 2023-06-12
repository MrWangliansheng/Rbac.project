using Rbac.project.Domain;

namespace Rbac.project.IRepoistory.Eextend
{
    public interface ILogDataRepoistory:IBaseRepoistory<LogData> 
    {
        void CreateLog(string name,string message,string oper);
    }
}
