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
using System.Net;
using System.Net.Sockets;

namespace Rbac.project.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        public readonly IUserRepoistory dal;
        public UserService(IUserRepoistory dal) : base(dal)
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
        /// 重置用户密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultDtoData ResetUserPasswrod(UserDto dto)
        {
            try
            {
                var i = dal.ResetUserPasswrod(dto.Id, dto.pwd);
                if (i>0)
                {
                    return new ResultDtoData { Result = Result.Success, Message = "重置密码成功" };
                }
                else
                {
                    return new ResultDtoData { Result = Result.Success, Message = "重置密码失败" };
                }
            }
            catch (Exception ex)
            {
                return new ResultDtoData {Result=Result.Error, Message = ex.Message };
            }
        }



        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultDtoData UpdateUser(User user)
        {
            try
            {
                var use = dal.Update(user);
                if (use.UserId > 0)
                {
                    return new ResultDtoData { Result = Result.Success, Message = "修改用户信息成功", Data = use };
                }
                else
                {
                    return new ResultDtoData { Result = Result.Success, Message = "修改用户信息失败" };
                }
            }
            catch (Exception ex)
            {
                return new ResultDtoData { Result = Result.Error, Message = ex.Message };
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
            try
            {
                var user = await dal.LogUser(name);
                if (user != null)
                {
                    if (user.UserPassword.ToUpper() == pwd.Md5().ToUpper())
                    {
                        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                        foreach (IPAddress addr in localIPs)
                        {
                            if (addr.AddressFamily == AddressFamily.InterNetwork)
                            {
                                user.LastLoginTime = DateTime.Now;
                                user.LastLoginIP = addr.ToString();
                                Idal.Update(user);
                            }
                        }
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
            catch (Exception ex)
            {
                return new ResultDto { Result=Result.Error, Message = ex.Message };
            }
        }
    }
}
