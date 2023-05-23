using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rbac.project.Domain;
using Rbac.project.IService;
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

        [HttpGet("UserLog")]
        public async Task<User> UserLog(string name, string pwd)
        {
            var user = bll.UserLog(name, pwd);
            return await user;
        }
    }
}
