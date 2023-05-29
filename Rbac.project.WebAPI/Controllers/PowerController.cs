using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rbac.project.Domain.Dto;
using Rbac.project.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rbac.project.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        private readonly IPowerService bll;
        public PowerController(IPowerService bll)
        {
            this.bll = bll;
        }
        /// <summary>
        /// 菜单权限级联选择器绑定
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPowerTree")]
        public IActionResult GetPowerTree()
        {
            var dto=bll.GetPowerTree();
            return Ok(dto);
        }
        /// <summary>
        /// 菜单树形结构列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetPowerTreeTableLevelone")]
        public async Task<ResultDtoData> GetPowerTreeTableLevelone(int id)
        {
            var resultlist=await bll.GetPowerTreeTableLevelone(id);
            return resultlist;
        }
        /// <summary>
        /// 查询菜单权限枚举
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPowerEnum")]
        public ResultDtoData GetPowerEnum()
        {
            return bll.GetPowerEnum();
        }
        /// <summary>
        /// 权限菜单级联绑定
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPowerTreeData")]
        public ResultDtoData GetPowerTreeData()
        {
            return bll.GetPowerTreeData();
        }
    }
}
