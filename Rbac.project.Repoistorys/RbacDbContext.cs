using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Repoistorys
{
    public class RbacDbContext: DbContext
    {
        public RbacDbContext(DbContextOptions<RbacDbContext> option):base(option) { }
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> User { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> Role { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public DbSet<UserRole> UserRole { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public DbSet<Power> Power { get; set; }
        /// <summary>
        /// 角色权限
        /// </summary>
        public DbSet<RolePower> RolePower { get; set; }
        
    }
}
