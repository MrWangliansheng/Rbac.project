using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using Rbac.project.IRepoistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Repoistorys
{
    public class RoleRepoistory: BaseRepoistory<Role>,IRoleRepoistory
    {
        private readonly RbacDbContext db;
        public RoleRepoistory(RbacDbContext db) : base(db)
        {
            this.db = db;
        }

        /// <summary>
        /// 异步添加数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<Role> InsertAsync(Role role)
        {
            var list =await db.Set<Role>().Where(m=>m.RoleName.Equals(role.RoleName)).ToListAsync();
            if (list.Count>0)
            {
                role.RoleId = -1;
                return role;
            }
            else
            {
                await db.Set<Role>().AddAsync(role);
                await db.SaveChangesAsync();
                return role;
            }
        }
    }
}
