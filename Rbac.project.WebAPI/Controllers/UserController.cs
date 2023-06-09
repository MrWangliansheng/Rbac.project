using AutoMapper;
using CSRedis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using NPOI.HSSF.Record.Chart;
using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using Rbac.project.IService;
using Rbac.project.IService.Eextend;
using Rbac.project.Service.Eextend;
using Rbac.project.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using static NPOI.HSSF.Util.HSSFColor;
using Rbac.project.WebAPI.Filter;

namespace Rbac.project.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(AuthorizeFilter))]
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
        [HttpGet("UserLog")]
        [AllowAnonymous]
        public async Task<ResultDto> UserLog([FromQuery]UserDto dto)
        {
            var code = await cs.GetAsync(dto.guid);
            if (string.IsNullOrEmpty(code))
            {
                return new ResultDto { Result = Result.Warning, Message = "验证码已失效" };
            }
            if (code.ToLower() != dto.code.ToLower())
            {
                return new ResultDto { Result = Result.Warning, Message = "验证码有误" };
            }
            var user =await bll.UserLog(dto.name, dto.pwd);
            
            return  user;
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
            return us;

        }
        /// <summary>
        /// 查询用户全部信息
        /// </summary>
        /// <returns></returns>
        //[HttpGet("GetUserAll")]
        //public async Task<ResultDtoData> GetUserAll()
        //{
        //    var list = await bll.GetALL();
        //    var result = new ResultDtoData { Result = Result.Success, Message = "数据查询成功", Data = list };
        //    return result;
        //}
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
            //var result = new ResultDtoData { Result = Result.Success, Message = "修改信息回显成功", Data = user };
            return user;
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
            var data = await bll.LogicDeleteAsync(id);
            return Ok(data);
        }
        /// <summary>
        /// 查询是否重复名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("GetUserName")]
        public async Task<IActionResult> GetUserName(string name)
        {
            var user =await bll.GetALL(m=>m.UserName.Equals(name));
            if (user.Count>0)
            {
                return Ok(new ResultDto { Result = Result.Warning });
            }
            else
            {
                return Ok(new ResultDto { Result = Result.Success });
            }
        }
        /// <summary>
        /// 获取新的token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("GetNewToken")]
        [AllowAnonymous]
        public ResultDtoData GetNewToken(string token)
        {
            return bll.GetNewToken(token);
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("Export")]
        public IActionResult Export(List<User> list)
        {
            var ex=list.ListToExcelPack();
            return File(ex, "application/ms-excel", "用户信息.xlsx");
        }
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("Import")]
        [AllowAnonymous]
        public IActionResult Import(IFormFile file)
        {
            var exname=Path.GetExtension(file.FileName);
            if (exname.ToLower()!=".xlsx"&&exname.ToLower()!=".xls")
            {
                return Ok(new ResultDto { Result = Result.Warning, Message = "格式有误" });
            }
            else
            {
                var guid = Guid.NewGuid()+exname;
                var path=Directory.GetCurrentDirectory()+"/wwwroot/Excel/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (Stream stream=new FileStream(path + guid, FileMode.Create))
                {
                    file.CopyTo(stream);
                    var list= stream.ExcelToListPack<User>(file.FileName);

                    
                    stream.Flush();
                }
                return null;
            }
        }
    }
}
