using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain.DataDisplay
{
    public class UserData:Inspect
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
       
        public string UserPassword { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
       
        public string UserEmail { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        
        public string UserImg { get; set; }
        /// <summary>
        /// 用户注册日期
        /// </summary>
        public DateTime UserCreateTime { get; set; }
        /// <summary>
        /// 用户描述
        /// </summary>
        public string UserDesc { get; set; }
        /// <summary>
        /// 用户名全称
        /// </summary>
        
        public string PullName { get; set; }
        /// <summary>        
        ///末次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>        
        ///末次登录IP
        /// </summary>
        [StringLength(100)]
        public string LastLoginIP { get; set; }
        /// <summary>
        /// 用户多个角色添加
        /// </summary>
        public List<int> RoleId { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public bool UserState { get; set; }
    }
}
