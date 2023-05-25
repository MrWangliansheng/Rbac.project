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
    public class RoleController : ControllerBase
    {

        public readonly IRoleService bll;
        public RoleController(IRoleService bll)
        {
            this.bll = bll;
        }
        /// <summary>
        ///查询所有角色
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRole")]
        public async Task<IActionResult> GetRole()
        {
            return Ok(new ResultDtoData { Result = Result.Success, Message = "角色信息查询成功" ,Data= await bll.GetALL() });
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
    }
}
