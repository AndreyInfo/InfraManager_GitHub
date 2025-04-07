using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    internal class ReadonlyRepository<TEntity> :
        IReadonlyRepository<TEntity>
        where TEntity : class
    {

        #region .ctor

        protected DbSet<TEntity> Set { get; }

        public ReadonlyRepository(DbSet<TEntity> set) : this(set, set.AsQueryable())
        {
        }

        protected ReadonlyRepository(DbSet<TEntity> set, IQueryable<TEntity> queryable)
        {
            Set = set;
            Queryable = queryable;
        }

        #endregion

        #region IEnumerable

        public IEnumerator<TEntity> GetEnumerator()
        {
            
            return Query().AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IAsyncEnumerable

        public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return Query().AsAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
        }

        #endregion

        #region IReadonlyRepository<TEntity>

        protected IQueryable<TEntity> Queryable { get; private set; }

        public virtual IExecutableQuery<TEntity> Query()
        {
            return new ExecutableQuery<TEntity>(Queryable);
        }

        public virtual IExecutableQuery<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return new ExecutableQuery<TEntity>(Queryable.Where(predicate));
        }

        public IReadonlyRepository<TEntity> DisableTrackingForQuery()
        {
            Queryable = Queryable.AsNoTracking();
            return this;
        }
        
        public Task<TEntity[]> ToArrayAsync(CancellationToken token = default)
        {
            return Query().ToArrayAsync(token);
        }

        public Task<TEntity[]> ToArrayAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return Query(predicate).ToArrayAsync(token);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return Query().CountAsync(cancellationToken);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Query(predicate).CountAsync(cancellationToken);
        }

        public Task<TEntity> FirstOrDefaultAsync(CancellationToken token = default)
        {
            return Query().FirstOrDefaultAsync(token);
        }

        public Task<TEntity> SingleOrDefaultAsync(CancellationToken token = default)
        {
            return Query().SingleOrDefaultAsync(token);
        }

        public Task<TEntity> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return Query().SingleOrDefaultAsync(predicate, token);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return Query(predicate).FirstOrDefaultAsync(token);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return Query(predicate).AnyAsync(cancellationToken: token);
        } 
        public Task<bool> AnyAsync(CancellationToken token = default)
        {
            return Query().AnyAsync(cancellationToken: token);
        }
        
        public Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return Query().AllAsync(predicate, cancellationToken: token);
        }

        public Task<T> MaxAsync<T>(Expression<Func<TEntity, T>> expression, CancellationToken cancellationToken = default)
        {
            return Query().MaxAsync(expression, cancellationToken);
        }

        public Task<T> MaxAsync<T>(Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, T>> maxExpression, CancellationToken cancellationToken = default)
        {
            return Query(whereExpression).MaxAsync(maxExpression, cancellationToken);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return Query(predicate).SingleAsync(token);
        }

        #endregion

        #region Include

        private class ReferenceIncludableRepository<TProperty>: 
            ReadonlyRepository<TEntity>,
            IIncludableRepository<TEntity, TProperty>
        {
            private readonly IIncludableQueryable<TEntity, TProperty> _query;

            public ReferenceIncludableRepository(
                DbSet<TEntity> set,
                IIncludableQueryable<TEntity, TProperty> query)
                : base(set, query)
            {
                _query = query;
            }

            public IIncludableRepository<TEntity, TNext> ThenWith<TNext>(
                Expression<Func<TProperty, TNext>> path)
            {
                return new ReferenceIncludableRepository<TNext>(
                    Set,
                    _query.ThenInclude(path));
            }

            public IIncludableRepository<TEntity, TNext> ThenWithMany<TNext>(
                Expression<Func<TProperty, IEnumerable<TNext>>> path)
            {
                return new CollectionIncludableRepository<TNext>(
                    Set,
                    _query.ThenInclude(path));
            }
        }

        private class CollectionIncludableRepository<TProperty>: 
            ReadonlyRepository<TEntity>,
            IIncludableRepository<TEntity, TProperty>
        {
            private readonly IIncludableQueryable<TEntity, IEnumerable<TProperty>> _query;

            public CollectionIncludableRepository(
                DbSet<TEntity> set,
                IIncludableQueryable<TEntity, IEnumerable<TProperty>> query)
                : base(set, query)
            {
                _query = query;
            }

            // TODO: Эти два метода - копипаста методов из класса выше
            public IIncludableRepository<TEntity, TNext> ThenWith<TNext>(Expression<Func<TProperty, TNext>> path)
            {
                return new ReferenceIncludableRepository<TNext>(
                    Set,
                    _query.ThenInclude(path));
            }

            public IIncludableRepository<TEntity, TNext> ThenWithMany<TNext>(Expression<Func<TProperty, IEnumerable<TNext>>> path)
            {
                return new CollectionIncludableRepository<TNext>(
                    Set,
                    _query.ThenInclude(path));
            }
        }

        public IIncludableRepository<TEntity, TProperty> With<TProperty>(
            Expression<Func<TEntity, TProperty>> path)
        {
            return new ReferenceIncludableRepository<TProperty>(Set, Queryable.Include(path));
        }

        public IIncludableRepository<TEntity, TProperty> WithMany<TProperty>(
            Expression<Func<TEntity, IEnumerable<TProperty>>> path)
        {
            return new CollectionIncludableRepository<TProperty>(Set, Queryable.Include(path));
        }

        #endregion
    }
}
