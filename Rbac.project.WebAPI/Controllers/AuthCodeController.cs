using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.IO;
using CSRedis;

namespace Rbac.project.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthCodeController : ControllerBase
    {
        public readonly CSRedisClient cs;
        public AuthCodeController(CSRedisClient cs)
        {
            this.cs = cs;
        }

        [HttpGet("UserCode")]
        public IActionResult UserCode(string guid)
        {
            var code = CreateCharCode(4);
            cs.SetAsync(guid, code).Wait();
            var autncode = CreateVerifyCode(code, 4);
            return File(autncode, "image/jpg");
        }
        /// <summary>
        /// 获取数字加字母验证码
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        //[HttpGet]
        public static string CreateCharCode(int n)
        {
            char[] strchar ={'a','b','c','d','e','f','g','h','i','j','k','l','m',
                          'n','o','p','q','r','s','t','u','v','w','x','y','z',
                          '0','1','2','3','4','5','6','7','8','9','A','B','C',
                          'D','E','F','G','H','I','3','K','L','M','N','O','p',
                          'o','R','s','T','U','v','w','x','Y','z'};

            string charCode = string.Empty;
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                charCode += strchar[random.Next(strchar.Length)];
            }
            return charCode;

        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        //[HttpGet]
        public static byte[] CreateVerifyCode(string charCode, int n)
        {
            int codeW = 170;//宽度
            int codeH = 50;//高度
            int fontSize = 32;//字体大小
            //初始化验证码
            //string charCode = string.Empty;
            string resultCode = "";

            //颜色列表
            Color[] colors = { Color.Black, Color.Red, Color.Green, Color.Blue };
            //字体列表
            string[] fonts = { "Times New Roman", "Verdana", "Arial", "Gungsuh" };
            //创建画布
            Bitmap bitmap = new Bitmap(codeW, codeH);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Random random = new Random();
            //画躁线
            for (int i = 0; i < n; i++)
            {
                int x1 = random.Next(codeW);
                int y1 = random.Next(codeH);
                int x2 = random.Next(codeW);
                int y2 = random.Next(codeH);

                Color color = colors[random.Next(colors.Length)];

                Pen pen = new Pen(color);
                graphics.DrawLine(pen, x1, y1, x2, y2);
            }
            //画躁点
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(codeW);
                int y = random.Next(codeH);
                Color color = colors[random.Next(colors.Length)];
                bitmap.SetPixel(x, y, color);
            }
            //画验证码
            for (int i = 0; i < n; i++)
            {
                string fontStr = fonts[random.Next(fonts.Length)];
                Font font = new Font(fontStr, fontSize);
                Color color = colors[random.Next(colors.Length)];
                graphics.DrawString(charCode[i].ToString(), font, new SolidBrush(color), (float)i * 30 + 5, (float)0);
            }
            try
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Jpeg);
                VerifyCode verifyCode = new VerifyCode()
                {
                    Image = stream.ToArray()
                };
                return stream.ToArray();
            }
            finally
            {
                graphics.Dispose();
                bitmap.Dispose();
            }
        }
    }
    public class VerifyCode
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 验证码数据流
        /// </summary>
        public byte[] Image { get; set; }

        public string Base64Str { get { return Convert.ToBase64String(Image); } }
    }
}
