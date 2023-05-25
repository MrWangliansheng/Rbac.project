using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IRepoistory
{
    public interface IUserRepoistory:IBaseRepoistory<User>
    {
        Task<User> LogUser(string name);
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        int ResetUserPasswrod (int userid,string password);
    }
}
