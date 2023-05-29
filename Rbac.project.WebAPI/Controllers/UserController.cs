using AutoMapper;
using CSRedis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;
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
        private readonly CSRedisClient cs;
        public readonly IUserService bll;
        private readonly IMapper mapper;
        public UserController(IUserService bll, CSRedisClient cs, IMapper mapper)
        {
            this.mapper = mapper;
            this.cs = cs;
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
            var code = await cs.GetAsync(dto.guid);
            if (code.ToLower() != dto.code.ToLower())
            {
                return new ResultDto { Result = Result.Warning, Message = "验证码有误" };
            }
            var user = bll.UserLog(dto.name, dto.pwd);
            return await user;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        public async Task<ResultDtoData> CreateUser(UserData user)
        {
            var us = await bll.InsertAsync(user);
            if (us.UserId > 0)
            {
                return new ResultDtoData { Result = Result.Success, Message = "用户添加成功" };
            }
            else if (us.UserId == -1)
            {
                return new ResultDtoData { Result = Result.Success, Message = "用户已存在" };
            }
            else
            {
                return new ResultDtoData { Result = Result.Warning, Message = "用户添加失败" };
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
            return result;
        }
        /// <summary>
        /// 分页查询用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("GetUserInfoPage")]
        public async Task<IActionResult> GetUserInfoPage([FromQuery] UserDto dto)
        {
            var result = await bll.GetUserInfoPage(dto);
            return Ok(result);
        }
        /// <summary>
        /// 反填修改用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("EditUser")]
        public async Task<ResultDtoData> EditUser(int id)
        {
            var user = await bll.FindAsync(id);
            var result = new ResultDtoData { Result = Result.Success, Message = "修改信息回显成功", Data = user };
            return result;
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        public ResultDtoData UpdateUser(UserData user)
        {
            var res = bll.UpdateUser(user);

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
        /// <summary>
        /// 逻辑删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("LogicDeleteAsyncUser")]
        public async Task<IActionResult> LogicDeleteAsyncUser(int id)
        {
            var user =  mapper.Map<User>(await bll.LogicDeleteAsync(id));
            if (user.UserIsDelete)
            {
                return Ok(new ResultDto { Result = Result.Success, Message = "删除成功" });
            }
            else
            {
                return Ok(new ResultDto { Result = Result.Success, Message = "删除失败" });
            }
        }
    }
}
