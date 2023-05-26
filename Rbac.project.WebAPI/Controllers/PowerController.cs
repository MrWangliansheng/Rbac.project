using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rbac.project.IService;

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
    }
}
