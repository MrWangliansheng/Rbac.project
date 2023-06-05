using CSRedis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;
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
        public RoleController(IRoleService bll, CSRedisClient cs)
        {
            this.cs = cs;
            this.bll = bll;
        }

        [HttpGet("GetRedis")]
        public async Task<IActionResult> GetRedis(string id = "aslkdjaoslkdjokasjndk")
        {
            cs.SetAsync("aaa", id).Wait();
            var aa = await cs.GetAsync("aaa");
            return Ok(aa);
        }
        /// <summary>
        ///查询所有角色
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRole")]
        public async Task<IActionResult> GetRole([FromQuery] RoleDto dto)
        {
            return Ok(await bll.GetRolePage(dto));
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(RoleData role)
        {
            var data = await bll.InsertAsync(role);
            return Ok(data);
        }
        /// <summary>
        /// 角色级联绑定
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRoleTree")]
        public IActionResult GetRoleTree()
        {
            var list = bll.GetRoleTree();
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
            var role = await bll.LogicDeleteAsync(id);
            return role;
        }
        /// <summary>
        /// 回显角色修改信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("EditRole")]
        public async Task<ResultDtoData> EditRole(int id)
        {
            var role=await bll.FindAsync(id);
            return role;
        }
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPut("UpdateRole")]
        public ResultDtoData UpdateRole(RoleData role)
        {
            var result = bll.Update(role);
            return result;
        }
        /// <summary>
        /// 查询角色名称是否重复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("GetRoleName")]
        public ResultDto GetRoleName(int id,string name)
        {
            var dto=bll.GetRoleName(id,name);
            return dto;
        }
    }
}
