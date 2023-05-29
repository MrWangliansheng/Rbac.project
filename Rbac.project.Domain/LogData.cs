using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain
{
    public class LogData
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        [Key]
        public int LogDataId { get; set; }
        /// <summary>
        /// 日志生成方法名称
        /// </summary>
        public string LogName { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string LogMessage { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 日志生成日期
        /// </summary>
        public DateTime DateTime { get; set; }= DateTime.Now;


    }
}
