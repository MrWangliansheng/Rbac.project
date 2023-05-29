﻿using Rbac.project.Domain.Enum;
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
    public class Power : Inspect
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
        /// 菜单上级ID
        /// </summary>
        public int PowerParentId { get; set; }

        /// <summary>
        /// 全部上级ID
        /// </summary>
        public string PowerParentIdAll { get; set; }
        /// <summary>
        /// 权限菜单类型
        /// </summary>
        public PowerEnum PowerType { get; set; }
        /// <summary>
        /// 权限创建日期
        /// </summary>
        public DateTime PowerTime { get; set; }= DateTime.Now;

        /// <summary>
        /// 菜单描述
        /// </summary>
        public string PowerDesc { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool PowerIsDelete { get; set; }
        /// <summary>
        /// 树形表格显示
        /// </summary>
        [NotMapped]
        public List<Power> children { get; set; }
    }
}
