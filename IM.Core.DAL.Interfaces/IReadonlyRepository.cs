using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public interface IReadonlyRepository<TEntity> : 
        IEnumerable<TEntity>,
        IAsyncEnumerable<TEntity> 
        where TEntity : class
    {
        IReadonlyRepository<TEntity> DisableTrackingForQuery(); //TODO: Выпилить
        
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity[]> ToArrayAsync(CancellationToken token = default);
        Task<TEntity[]> ToArrayAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,  CancellationToken token = default);
        Task<TEntity> FirstOrDefaultAsync(CancellationToken token = default);
        Task<TEntity> SingleOrDefaultAsync(CancellationToken cancellationToken = default);
        Task<TEntity> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> MaxAsync<T>(Expression<Func<TEntity, T>> expression, CancellationToken cancellationToken = default);

        Task<T> MaxAsync<T>(Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, T>> maxExpression, CancellationToken cancellationToken = default);
        
        IExecutableQuery<TEntity> Query();
        IExecutableQuery<TEntity> Query(Expression<Func<TEntity, bool>> predicate);

        IIncludableRepository<TEntity, TProperty> With<TProperty>(
            Expression<Func<TEntity, TProperty>> path);
        IIncludableRepository<TEntity, TProperty> WithMany<TProperty>(
            Expression<Func<TEntity, IEnumerable<TProperty>>> path);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
    }
}
