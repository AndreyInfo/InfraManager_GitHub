using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InfraManager.DAL.DeleteStrategies
{
    internal class PhysicalDeleteStrategy<TEntity> : CascadeDeleteStrategy<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        public PhysicalDeleteStrategy(
            DbSet<TEntity> dbSet, 
            IEnumerable<IDependentDeleteStrategy<TEntity>> dependencies) : base(dependencies)
        {
            _dbSet = dbSet;
        }

        protected override void DeleteEntity(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
