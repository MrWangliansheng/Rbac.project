using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain
{
    [Table("Role")]
    public class Role : Inspect
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(50)]
        public string RoleName { get; set; }
        /// <summary>
        /// 角色添加日期
        /// </summary>
        public DateTime RoleCreateTime { get; set; }
    }
}
