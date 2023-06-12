

namespace Rbac.project.Domain.Dto
{
    public class ResultDto
    {
        /// <summary>
        /// 状态
        /// </summary>
        public Result? Result { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string? Message { get;set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Key { get; set; }

    }
    public class ResultDtoData : ResultDto
    {
        /// <summary>
        /// 实体信息
        /// </summary>
        public object? Data { get; set; }
    }

    public enum Result
    {
        Success=200,
        Error=500,
        Warning=100,//警告
    }
}
