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
using AutoMapper;

using Microsoft.Extensions.DependencyInjection;
using Rbac.project.Repoistorys.AutoMapper;
using Rbac.project.Domain.DataDisplay;

namespace Rbac.project.Repoistorys
{
    public class UserRepoistory : BaseRepoistory<User>, IUserRepoistory
    {
        private readonly RbacDbContext db;
        private readonly IMapper mapper;
        public UserRepoistory(RbacDbContext db, IMapper mapper) : base(db)
        {
            this.mapper = mapper;
            this.db = db;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<User> LogUser(string name)
        {
            var user = await db.User.Where(m => m.UserName.Equals(name)).FirstOrDefaultAsync();

            return user;
        }
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int ResetUserPasswrod(int userid, string password)
        {
            var user = db.Set<User>().Find(userid);
            user.UserPassword = password.Md5();
            var i = db.SaveChanges();
            return i;
        }

        public override async Task<User> InsertAsync(User User)
        {
            try
            {
                var list = await db.Set<User>().Where(m => m.UserName.Equals(User.UserName)).ToListAsync();
                if (list.Count > 0)
                {
                    User.UserId = -1;
                    return User;
                }
                else
                {
                    await db.AddAsync(User);
                    await db.SaveChangesAsync();
                    return User;
                }
            }
            catch (Exception)
            {

               return User = null;
            }
           
        }
        /// <summary>
        /// 分页查询用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PageDto> GetUserInfoPage(UserDto dto)
        {
            var list = db.Set<User>().Where(m=>m.UserIsDelete.Equals(false)).AsQueryable();

            if (!string.IsNullOrEmpty(dto.name))
            {
                list = list.Where(m => m.PullName.Contains(dto.name) || m.UserName.Contains(dto.name));
            }
            if (dto.createstartime != null)
            {
                list = list.Where(m => m.UserCreateTime >= dto.createstartime);
            }
            if (dto.createendtime != null)
            {
                list = list.Where(m => m.UserCreateTime <= dto.createendtime);
            }

            dto.total = list.Count();
            dto.pagecount = (int)Math.Ceiling(dto.total * 1.0 / dto.pagesize);

            list = list.Skip((dto.pageindex - 1) * dto.pagesize).Take(dto.pagesize);

            return new PageDto { Result = Result.Success, Message = "用户分页信息查询成功", Data = mapper.Map<List<UserData>>(await list.ToListAsync()), total = dto.total, pagecount = dto.pagecount };
        }
        /// <summary>
        /// 逻辑删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<User> LogicDeleteAsync(int id)
        {
            var user =await db.User.FindAsync(id);
            user.UserIsDelete = true;
            await db.SaveChangesAsync();
            return user;
        }
    }
}
