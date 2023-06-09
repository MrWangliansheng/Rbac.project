﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        ///角色上级ID 
        /// </summary>
        public int RoleParentId { get; set; }
        /// <summary>
        /// 全部上级ID
        /// </summary>
        public string RoleParentIdAll { get;set; }
        /// <summary>
        /// 角色添加日期
        /// </summary>
        public DateTime RoleCreateTime { get; set; }=DateTime.Now;
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool RoleIsDelete { get; set; }
    }
}
