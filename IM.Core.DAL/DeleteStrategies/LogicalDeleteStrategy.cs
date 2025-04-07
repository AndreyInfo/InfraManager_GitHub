using System.Collections.Generic;

namespace InfraManager.DAL.DeleteStrategies
{
    internal class LogicalDeleteStrategy<TEntity> : CascadeDeleteStrategy<TEntity>
        where TEntity : IMarkableForDelete
    {
        public LogicalDeleteStrategy(IEnumerable<IDependentDeleteStrategy<TEntity>> dependencies) : base(dependencies)
        {
        }

        protected override void DeleteEntity(TEntity entity)
        {
            entity.MarkForDelete();
        }
    }
}
