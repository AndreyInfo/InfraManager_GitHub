using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.EntityFieldEditor
{
    public interface IFieldAccessor<TEntity, TData> : IFieldAccessor<TEntity>
        where TEntity : class
    {
        bool MatchesCurrent(TEntity entity, TData data);
        BaseError SetValue(TEntity entity, TData data);
    }

    public interface IFieldAccessor<TEntity> where TEntity : class
    {
        object GetValue(TEntity entity);
    }
}
