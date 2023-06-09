﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rbac.project.Domain.ParentIdAll
{
    [Table("RolePowerIdAll")]
    public class RolePowerIdAll
    {
        /// <summary>
        /// 角色所有菜单
        /// </summary>
        [Key]
        public int RolePowerIdAllId { get; set; }
        /// <summary>
        /// 角色 ID
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 菜单全部ID
        /// </summary>
        public string PowerIdAll { get; set; }
    }
}
