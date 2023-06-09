using Rbac.project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain.DataDisplay
{
    public class PowerData:Inspect
    {

        /// <summary>
        /// 权限ID
        /// </summary>
        public int PowerId { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string PowerName { get; set; }
        /// <summary>
        /// 权限路由
        /// </summary>
        public string PowerRoute { get; set; }
        /// <summary>
        /// 权限路由名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 菜单上级ID
        /// </summary>
        public int PowerParentId { get; set; }

        /// <summary>
        /// 全部上级ID
        /// </summary>
        public string? PowerParentIdAll { get; set; }
        /// <summary>
        /// 权限菜单类型
        /// </summary>
        public PowerEnum PowerType { get; set; }

        /// <summary>
        /// 菜单描述
        /// </summary>
        public string PowerDesc { get; set; }
        /// <summary>
        /// 接口Url
        /// </summary>
        public string? PowerAPIUrl { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string PowerIcon { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool PowerIsDelete { get; set; }
        /// <summary>
        /// 重定向路由
        /// </summary>
        public string PowerRedirect { get; set; }
    }
}
