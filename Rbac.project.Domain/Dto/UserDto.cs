using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain.Dto
{
    public class UserDto:PageDto
    {
        /// <summary>
        /// 用户ID 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? pwd { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string? code { get; set; }
        /// <summary>
        /// 全球唯一标识符
        /// </summary>
        public string guid { get; set; }
        /// <summary>
        /// 创建开始时间
        /// </summary>
        public DateTime? createstartime { get; set; }
        /// <summary>
        /// 创建结束时间
        /// </summary>
        public DateTime? createendtime { get; set; }
        
    }
}
