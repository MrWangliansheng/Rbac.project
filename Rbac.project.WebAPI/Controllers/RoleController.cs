using CSRedis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using Rbac.project.IService;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Rbac.project.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        public readonly CSRedisClient cs;
        public readonly IRoleService bll;
        public RoleController(IRoleService bll,CSRedisClient cs)
        {
            this.cs = cs;
            this.bll = bll;
        }

        [HttpGet("GetRedis")]
        public async Task<IActionResult> GetRedis(string id="aslkdjaoslkdjokasjndk")
        {
            cs.SetAsync("aaa",id).Wait();
            var aa =await cs.GetAsync("aaa");
            return Ok(aa);
        }
        /// <summary>
        ///查询所有角色
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRole")]
        public async Task<IActionResult> GetRole([FromQuery]RoleDto dto)
        {
            return Ok(await bll.GetRolePage(dto));
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(Role role)
        {
            await bll.InsertAsync(role);
            if (role.RoleId>0)
            {
                return Ok(new ResultDtoData { Result = Result.Success, Message = "添加成功" });
            }
            else if(role.RoleId ==-1) 
            {
                return Ok(new ResultDtoData { Result = Result.Success, Message = "角色已存在" ,Data=role});
            }
            else
            {
                return Ok(new ResultDtoData { Result = Result.Success, Message = "添加失败" });
            }
        }
        /// <summary>
        /// 角色级联绑定
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRoleTree")]
        public IActionResult GetRoleTree()
        {
            var list=bll.GetRoleTree();
            return Ok(list);
        }
        /// <summary>
        /// 逻辑删除角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("LogicDeleteAsyncRole")]
        public async Task<ResultDto> LogicDeleteAsyncRole(int id)
        {
            var role =await bll.LogicDeleteAsync(id);
            return role;
            //if (role.RoleIsDelete)
            //{
            //    return new ResultDto { Result = Result.Success, Message = "删除成功" };
            //}
            //else
            //{
            //    return new ResultDto { Result = Result.Success, Message = "删除失败" };
            //}
        }
    }
}
