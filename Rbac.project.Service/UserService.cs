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
using Rbac.project.Domain.DataDisplay;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
namespace Rbac.project.Service
{
    public class UserService : BaseService<UserData, ResultDtoData>, IUserService
    {
        public readonly IUserRepoistory dal;
        public readonly IMapper mapper;
        public readonly IConfiguration configuration;
        public UserService(IUserRepoistory dal, IMapper mapper, IConfiguration configuration) : base(dal, mapper)
        {
            this.mapper = mapper;
            this.dal = dal;
            this.configuration = configuration;
        }

        /// <summary>
        /// 查询全部用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAll()
        {
            try
            {
                var list = await dal.GetALL();
                return mapper.Map<List<User>>(list);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 分页查询用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PageDto> GetUserInfoPage(UserDto dto)
        {
            return await dal.GetUserInfoPage(dto);
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
                if (i > 0)
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
                return new ResultDtoData { Result = Result.Error, Message = ex.Message };
            }
        }


        public override async Task<ResultDtoData> FindAsync(int id)
        {
            var user = await Idal.FindAsync(id);
            if (user != null)
            {
                return new ResultDtoData { Result = Result.Success, Message = "", Data = user };
            }
            else
            {
                return new ResultDtoData { Result = Result.Error, Message = "信息回显异常", Data = user };
            }
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResultDtoData UpdateUser(UserData user)
        {
            try
            {
                var use = dal.Update(user);
                if (use != null)
                {
                    if (use.UserId > 0)
                    {
                        return new ResultDtoData { Result = Result.Success, Message = "修改用户信息成功", Data = use };
                    }
                    else if (use.UserId == -1)
                    {
                        return new ResultDtoData { Result = Result.Warning, Message = "用户名已存在无法修修改" };
                    }
                    else
                    {
                        return new ResultDtoData { Result = Result.Warning, Message = "修改用户信息失败" };
                    }
                }
                else
                {
                    return new ResultDtoData { Result = Result.Error, Message = "修改用户出现异常详情请查看日志" };
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
                                Idal.Update(mapper.Map<UserData>(user));
                            }
                        }
                        //生成Token
                        SecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Kestrel:key"]));
                        IList<Claim> claims = new List<Claim> {
                            new Claim(JwtClaimTypes.Id,user.UserId.ToString()),
                            new Claim(JwtClaimTypes.Name,user.UserName.ToString()),
                            new Claim(JwtClaimTypes.Email,user.UserEmail.ToString()),
                        };
                        SecurityToken token = new JwtSecurityToken(issuer: "issuer", audience: "audience", signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                            expires: DateTime.Now.AddSeconds(10),
                            claims: claims);
                        return new ResultDto { Result = Result.Success, Message = "登陆成功", Key = "Bearer " + new JwtSecurityTokenHandler().WriteToken(token) };
                    }
                    else
                    {
                        return new ResultDto { Result = Result.Success, Message = "登录失败用户名或密码错误" };
                    }
                }
                else
                {
                    return new ResultDto { Result = Result.Warning, Message = "无账户信息" };
                }
            }
            catch (Exception ex)
            {
                return new ResultDto { Result = Result.Error, Message = ex.Message };
            }
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override async Task<ResultDtoData> InsertAsync(UserData data)
        {

            try
            {
                //var user=mapper.Map<User>(data);
                var user = await Idal.InsertAsync(data);
                if (user != null)
                {
                    return new ResultDtoData { Result = Result.Success, Message = "添加用户成功" };
                }
                else
                {
                    return new ResultDtoData { Result = Result.Error, Message = "添加用户异常详细请查看日志" };
                }
            }
            catch (Exception ex)
            {
                return new ResultDtoData { Result = Result.Error, Message = "添加用户异常详细请查看日志" };
            }
        }

        public ResultDtoData GetNewToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var payload = handler.ReadJwtToken(token).Payload;
                SecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Kestrel:key"]));
                IList<Claim> claims = payload.Claims.Where(m=>m.Type!="and").ToList();
                SecurityToken newtoken = new JwtSecurityToken( signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                    expires: DateTime.Now.AddSeconds(10),
                    claims: claims);
                return new ResultDtoData { Result = Result.Success, Key = "Bearer " + new JwtSecurityTokenHandler().WriteToken(newtoken) };
            }
            catch (Exception)
            {
                return new ResultDtoData { Result = Result.Error };
            }
           
        }
    }
}
