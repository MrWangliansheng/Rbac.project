using Microsoft.EntityFrameworkCore;
using Rbac.project.Repoistorys;
using System;
using System.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        public readonly RbacDbContext db;
        public Program(RbacDbContext db)
        {
            this.db = db;
        }

        public  void Main()
        {
            var list = db.User.ToList();
            Console.WriteLine(list);
        }
    }
}
