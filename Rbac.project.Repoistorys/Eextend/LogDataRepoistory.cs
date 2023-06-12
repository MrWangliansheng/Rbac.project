using Rbac.project.Domain;
using Rbac.project.IRepoistory.Eextend;
using Rbac.project.Repoistorys;

namespace Rbac.project.Repoistory.Eextend
{
    public class LogDataRepoistory:BaseRepoistory<LogData>,ILogDataRepoistory
    {
        private readonly RbacDbContext db;
        public LogDataRepoistory(RbacDbContext db):base(db)
        {
            this.db = db;
        }

        public void CreateLog(string name, string message, string oper)
        {
            var log=new LogData {LogDataId=0, LogName=name,LogMessage=message,Operator=oper};
            db.Add(log);
            db.SaveChanges();
        }
    }
}
