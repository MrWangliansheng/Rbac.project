using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using Rbac.project.IRepoistory;
using Rbac.project.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rbac.project.Utility;
using System.Security.Cryptography;
using Rbac.project.Domain.Dto;

namespace Rbac.project.Repoistorys
{
    public class UserRepoistory : BaseRepoistory<User>, IUserRepoistory
    {
        private readonly RbacDbContext db;
        public UserRepoistory(RbacDbContext db) : base(db)
        {
            this.db=db;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<User> LogUser(string name)
        {
            var user =await db.User.Where(m => m.UserName.Equals(name)).FirstOrDefaultAsync();
           
            return  user;
        }
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int ResetUserPasswrod(int userid, string password)
        {
            var user= db.Set<User>().Find(userid);
            user.UserPassword=password.Md5();
            var i= db.SaveChanges();
            return i;
        }

        public override async Task<User> InsertAsync(User User)
        {
            var list =await db.Set<User>().Where(m => m.UserName.Equals(User.UserName)).ToListAsync();
            if (list.Count>0)
            {
                User.UserId = -1;
                return  User;
            }
            else
            {
                return User;
            }
        }
    }
}
