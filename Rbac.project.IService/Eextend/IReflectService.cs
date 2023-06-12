
namespace Rbac.project.IService.Eextend
{
    public interface IReflectService<TEntity> where TEntity : class
    {
        void CreateAudit(TEntity entity, string name);
        void DeleteAudit(TEntity entity, string name);
        void UpdateAudit(TEntity entity, string name);
    }
}
