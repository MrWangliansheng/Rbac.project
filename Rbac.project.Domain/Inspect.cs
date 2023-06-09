﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Rbac.project.Domain
{
    public class Inspect
    {
        /// <summary>
        /// 信息创建人
        /// </summary>
        [StringLength(50)]
        public string? MesgCreateUser { get; set; }
        /// <summary>
        /// 信息创建日期
        /// </summary>
        public DateTime? MsgCreateTime { get; set; }
        /// <summary>
        /// 信息删除人
        /// </summary>
        [StringLength(50)]
        public string? MegDeleteUser { get; set; }
        /// <summary>
        /// 信息删除日期
        /// </summary>
        public DateTime? MsgDeleteTime { get; set; }
        /// <summary>
        /// 信息修改人
        /// </summary>
        [StringLength(50)]
        public string? MsgUpdateUser { get; set; }
        /// <summary>
        /// 信息修改日期
        /// </summary>
        public DateTime? MegUpdateTipme { get; set; }
    }
}
