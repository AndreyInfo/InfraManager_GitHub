using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL
{
    internal class Repository<TEntity> : 
        ReadonlyRepository<TEntity>, 
        IRepository<TEntity> 
        where TEntity : class
    {
        private readonly IDeleteStrategy<TEntity> _deleteStrategy;

        public Repository(
            DbSet<TEntity> set, 
            IDeleteStrategy<TEntity> deleteStrategy)
            : base(set)
        {
            _deleteStrategy = deleteStrategy;
        }

        public void Insert(TEntity entity)
        {
            Set.Add(entity);
            Inserted(entity);
        }

        protected virtual void Inserted(TEntity entity)
        {
            // по умолчанию ничего не делаем
        }

        public void Attach(TEntity entity)
        {
            Set.Attach(entity);
        }

        public void Delete(TEntity entity)
        {
            _deleteStrategy.Delete(entity);
        }
    }
}