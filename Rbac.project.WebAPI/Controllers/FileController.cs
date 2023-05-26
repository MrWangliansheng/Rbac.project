using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rbac.project.Domain.Dto;
using System;
using System.IO;

namespace Rbac.project.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpPost("ImgUp")]
        public ResultDto ImgUp()
        {
            try
            {
                var file = HttpContext.Request.Form.Files[0];
                var exname=Path.GetExtension(file.FileName);
                if (exname.ToLower()!= ".jpg" && exname.ToLower() != ".jpeg" && exname.ToLower() != ".png" && exname.ToLower() != ".webp")
                {
                    return new ResultDto { Result = Result.Warning, Message = "格式有误" };
                }
                var guid = DateTime.Now.ToString("yyyyMMdd")+"/"+Guid.NewGuid().ToString() ;

                var path = Directory.GetCurrentDirectory()+"/wwwroot/img/"+guid;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream fs=new FileStream(path + exname, FileMode.Create))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return new ResultDto { Result = Result.Success, Message = guid+ exname }; 
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
