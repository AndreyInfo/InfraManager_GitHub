using System.Collections.Generic;

namespace InfraManager.DAL.DeleteStrategies
{
    internal abstract class CascadeDeleteStrategy<TEntity> : IDeleteStrategy<TEntity>
    {
        private readonly IEnumerable<IDependentDeleteStrategy<TEntity>> _dependencies;

        protected CascadeDeleteStrategy(IEnumerable<IDependentDeleteStrategy<TEntity>> dependencies)
        {
            _dependencies = dependencies;
        }

        public void Delete(TEntity entity)
        {
            _dependencies.ForEach(x => x.OnDelete(entity));
            DeleteEntity(entity);
        }

        protected abstract void DeleteEntity(TEntity entity);
    }
}
