using Rbac.project.IRepoistory.Eextend;
using Rbac.project.IService.Eextend;

namespace Rbac.project.Service.Eextend
{
    public class ReflectService<TEntity>: IReflectService<TEntity> where TEntity : class
    {
        public readonly IReflectRepoistory<TEntity> dal;
        public ReflectService(IReflectRepoistory<TEntity> dal)
        {
            this.dal = dal;
        }
        /// <summary>
        /// 添加审计字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="name"></param>
        public void CreateAudit(TEntity entity, string name)
        {
            dal.CreateAudit(entity, name);
        }
        /// <summary>
        /// 删除审计字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="name"></param>
        public void DeleteAudit(TEntity entity, string name)
        {
            dal.DeleteAudit(entity, name);

        }
        /// <summary>
        /// 修改审计字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="name"></param>
        public void UpdateAudit(TEntity entity, string name)
        {
            dal.UpdateAudit(entity, name);

        }
    }
}
