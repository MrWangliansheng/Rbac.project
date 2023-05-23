using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rbac.project.Domain
{
    [Table("User")]
    public class User : Inspect
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [StringLength(50)]
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [StringLength (50)]
        public string UserPwaword { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        [StringLength(100)]
        public string UserEmail { get; set; }
        /// <summary>
        /// 用户注册日期
        /// </summary>
        public DateTime UserCreateTime { get; set; }
    }
}
