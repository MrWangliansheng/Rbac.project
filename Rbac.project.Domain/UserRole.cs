using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain
{
    [Table("UserRole")]
    public class UserRole
    {
        /// <summary>
        /// 用户角色中间表ID
        /// </summary>
        public int UserRoleId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        ///// <summary>
        ///// 用户ID
        ///// </summary>
        //[ForeignKey("UserID")]
        //public User User { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
        //[ForeignKey("RoleID")]
        //public Role Role { get; set; }
    }
}
