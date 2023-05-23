using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using Rbac.project.IService;
using System.Collections.Generic;
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
        [HttpGet("UserLog")]
        public async Task<ResultDto> UserLog(string name, string pwd)
        {
            var user = bll.UserLog(name, pwd);
            return await user;
        }
        /// <summary>
        /// 查询用户全部信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserAll")]
        public async Task<List<User>> GetUserAll()
        {
            var list = bll.GetALL();
            return await list;
        }
    }
}
