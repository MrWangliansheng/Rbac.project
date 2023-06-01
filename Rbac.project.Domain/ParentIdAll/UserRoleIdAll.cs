using System;


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain.ParentIdAll
{
    [Table("UserRoleIdAll")]
    public class UserRoleIdAll
    {
        /// <summary>
        /// 用户角色上级全部ID
        /// </summary>
        [Key]
        public int URAID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 全部角色ID
        /// </summary>
        public string RoleIdAll { get; set; }
    }
}
