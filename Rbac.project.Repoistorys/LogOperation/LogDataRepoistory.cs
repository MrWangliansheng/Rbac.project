using Rbac.project.Domain;
using Rbac.project.IRepoistory.LogOperation;
using Rbac.project.Repoistorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Repoistory.LogOperation
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
