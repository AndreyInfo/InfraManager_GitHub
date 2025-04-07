using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.DeleteStrategies
{
    internal class ObjectIconDepencyDeleteStrategy<T> : IDependentDeleteStrategy<T>
        where T : class, IGloballyIdentifiedEntity
    {
        private readonly DbSet<ObjectIcon> _dbSet;

        public ObjectIconDepencyDeleteStrategy(DbSet<ObjectIcon> dbSet)
        {
            _dbSet = dbSet;
        }

        public void OnDelete(T entity)
        {
            _dbSet.RemoveObjectIconIfExists(entity);
        }
    }
}
