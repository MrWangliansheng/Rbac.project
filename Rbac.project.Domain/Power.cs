using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain
{
    [Table("Power")]
    public class Power:Inspect
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public int PowerId { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        [StringLength(50)]
        public string PowerName { get; set; }   
        /// <summary>
        /// 权限路由
        /// </summary>
        [StringLength(50)]
        public string PowerRoute { get; set; }
        /// <summary>
        /// 权限创建日期
        /// </summary>
        public DateTime PowerTime { get; set; }
    }
}
