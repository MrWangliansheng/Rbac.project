using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using Rbac.project.IService;
using Rbac.project.Utility;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Rbac.project.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService bll;
        public UserController(IUserService bll)
        {
            this.bll = bll;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost("UserLog")]
        public async Task<ResultDto> UserLog(UserDto dto)
        {
            var user = bll.UserLog(dto.name, dto.pwd);
            return await user;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        public async Task<ResultDtoData> CreateUser(User user)
        {
            var us=await bll.InsertAsync(user);
            if (us.UserId>0)
            {
                return new ResultDtoData { Result = Result.Success, Message = "用户添加成功" };
            }
            else if(us.UserId==-1)
            {
                return new ResultDtoData { Result = Result.Success, Message = "用户已存在" };
            }
            else
            {
                return new ResultDtoData { Result = Result.Success, Message = "用户添加失败" };
            }
            
        }
        /// <summary>
        /// 查询用户全部信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserAll")]
        public async Task<ResultDtoData> GetUserAll()
        {
            var list = await bll.GetALL();
            var result = new ResultDtoData { Result = Result.Success, Message = "数据查询成功", Data = list };
            return  result;
        }
        /// <summary>
        /// 反填修改用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("EditUser")]
        public async Task<ResultDtoData> EditUser(int id)
        {
            var user= await bll.FindAsync(id);
            var result = new ResultDtoData { Result = Result.Success, Message = "修改信息回显成功", Data = user };
            return result;
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        public ResultDtoData UpdateUser(User user)
        {
            var res=bll.UpdateUser(user);

            return res;
        }
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("ResetUserPasswrod")]
        public ResultDtoData ResetUserPasswrod(UserDto dto)
        {
            return bll.ResetUserPasswrod(dto);
        }
    }
}
