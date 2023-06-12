using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using Rbac.project.Domain.ParentIdAll;

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
        /// <summary>
        /// 日志信息表
        /// </summary>
        public DbSet<LogData> LogData { get; set; }
        /// <summary>
        /// 用户角色全部ID
        /// </summary>
        public  DbSet<UserRoleIdAll> UserRoleIdAll { get; set;}
        /// <summary>
        /// 角色菜单全部ID
        /// </summary>
        public DbSet<RolePowerIdAll> RolePowerIdAll { get; set;}
    }
}
