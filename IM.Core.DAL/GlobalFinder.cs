using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    internal class GlobalFinder<TEntity> :
        IFindEntityByGlobalIdentifier<TEntity> 
        where TEntity : class, IGloballyIdentifiedEntity
    {
        #region .ctor

        protected DbSet<TEntity> Set { get; }

        public GlobalFinder(DbSet<TEntity> set)
        {
            Set = set;
        }

        #endregion

        #region Finder

        public TEntity Find(Guid globalId) => Query.Single(x => x.IMObjID == globalId);

        public Task<TEntity> FindAsync(Guid globalId, CancellationToken token = default)
        {
            return Query.SingleOrDefaultAsync(x => x.IMObjID == globalId, token);
        }

        protected virtual IQueryable<TEntity> Query => Set.AsQueryable();

        #endregion

        #region Include

        //TODO: Refactor похожее решение для Include в Repository
        private class ReferenceIncludableGlobalFinder<TProperty> :
            GlobalFinder<TEntity>,
            IIncludableGlobalIdentifiedEntityFinder<TEntity, TProperty>
        {
            public ReferenceIncludableGlobalFinder(
                DbSet<TEntity> set,
                IIncludableQueryable<TEntity, TProperty> includableQuery) 
                : base(set)
            {
                _includableQuery = includableQuery;
            }

            private readonly IIncludableQueryable<TEntity, TProperty> _includableQuery;

            protected override IQueryable<TEntity> Query => _includableQuery;

            public IIncludableGlobalIdentifiedEntityFinder<TEntity, TNext> ThenWith<TNext>(
                Expression<Func<TProperty, TNext>> path) =>
                     new ReferenceIncludableGlobalFinder<TNext>(
                        Set,
                        _includableQuery.ThenInclude(path));

            public IIncludableGlobalIdentifiedEntityFinder<TEntity, TNext> ThenWithMany<TNext>(
                Expression<Func<TProperty, IEnumerable<TNext>>> path) =>
                     new CollectionIncludedableGlobalFinder<TNext>(
                        Set,
                        _includableQuery.ThenInclude(path));
        }

        private class CollectionIncludedableGlobalFinder<TProperty> :
            GlobalFinder<TEntity>,
            IIncludableGlobalIdentifiedEntityFinder<TEntity, TProperty>
        {
            private readonly IIncludableQueryable<TEntity, IEnumerable<TProperty>> _includableQuery;

            public CollectionIncludedableGlobalFinder(
                DbSet<TEntity> set,
                IIncludableQueryable<TEntity, IEnumerable<TProperty>> includableQuery)
                : base(set)
            {
                _includableQuery = includableQuery;
            }

            protected override IQueryable<TEntity> Query => _includableQuery;

            // TODO: Эти два метода - копипаста с класса выше
            public IIncludableGlobalIdentifiedEntityFinder<TEntity, TNext> ThenWith<TNext>(
                Expression<Func<TProperty, TNext>> path) =>
                     new ReferenceIncludableGlobalFinder<TNext>(
                        Set,
                        _includableQuery.ThenInclude(path));

            public IIncludableGlobalIdentifiedEntityFinder<TEntity, TNext> ThenWithMany<TNext>(
                Expression<Func<TProperty, IEnumerable<TNext>>> path) =>
                     new CollectionIncludedableGlobalFinder<TNext>(
                        Set,
                        _includableQuery.ThenInclude(path));
        }

        public IIncludableGlobalIdentifiedEntityFinder<TEntity, TProperty> With<TProperty>(
            Expression<Func<TEntity, TProperty>> path) =>
                new ReferenceIncludableGlobalFinder<TProperty>(Set, Query.Include(path));

        public IIncludableGlobalIdentifiedEntityFinder<TEntity, TProperty> WithMany<TProperty>(
            Expression<Func<TEntity, IEnumerable<TProperty>>> path) =>
                new CollectionIncludedableGlobalFinder<TProperty>(Set, Query.Include(path));

        async Task<IGloballyIdentifiedEntity> IFindEntityByGlobalIdentifier.FindAsync(Guid globalID, CancellationToken token)
        {
            return await FindAsync(globalID, token);
        }

        #endregion
    }
}
