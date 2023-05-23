using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using Rbac.project.IRepoistory;
using Rbac.project.IService;
using Rbac.project.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        public readonly IUserRepoistory dal;
        public UserService(IUserRepoistory dal):base(dal)
        {
            this.dal = dal;
        }
        /// <summary>
        /// 查询全部用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAll()
        {
            try
            {
                var list = dal.GetALL();
                return await list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public async Task<ResultDto> UserLog(string name, string pwd)
        {
            var user = await dal.LogUser(name);
            if (user != null)
            {
                if (user.UserPwaword.ToUpper() == pwd.Md5().ToUpper())
                {
                    return new ResultDto { Result = Result.Success, Message = "登陆成功" };
                }
                else
                {
                    return new ResultDto { Result = Result.Success, Message = "登录失败" };
                }
            }
            else
            {
                return new ResultDto { Result = Result.Success, Message = "无账户信息" };
            }
        }
    }
}
