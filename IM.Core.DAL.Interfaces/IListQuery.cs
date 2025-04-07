using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL
{
    public interface IListQuery<TEntity, TQueryResult>
    {
        IQueryable<TQueryResult> Query(
            Guid userId,
            IEnumerable<Expression<Func<TEntity, bool>>> predicates);
    }

    public interface IListQuery<TEntity, TSubQuery, TResultQuery>
    {
        IQueryable<TResultQuery> Query(
            Guid userId,
            IQueryable<TSubQuery> subQuery,
            IEnumerable<Expression<Func<TEntity, bool>>> predicates);
    }
    
}
