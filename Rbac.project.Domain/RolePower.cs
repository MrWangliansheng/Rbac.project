
using System.ComponentModel.DataAnnotations.Schema;

namespace Rbac.project.Domain
{
    [Table("RolePower")]
    public class RolePower
    {
        /// <summary>
        /// 角色权限ID
        /// </summary>
        public int RolePowerId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 权限ID
        /// </summary>
        public int PowerID { get; set; }
    }
}
