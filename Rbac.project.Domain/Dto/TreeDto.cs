using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain.Dto
{
    public class TreeDto
    {
        /// <summary>
        /// 树形值
        /// </summary>
        public int value { get;set; }
        /// <summary>
        /// 显示内容
        /// </summary>
        public string label { get; set; }
        /// <summary>
        /// 子节点显示
        /// </summary>
        public List<TreeDto> children { get; set;}
    }
}
