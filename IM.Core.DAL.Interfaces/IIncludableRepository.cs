using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.DAL
{
    public interface IIncludableRepository<TEntity, TProperty> : IReadonlyRepository<TEntity>
        where TEntity : class
    {
        IIncludableRepository<TEntity, TNext> ThenWith<TNext>(
            Expression<Func<TProperty, TNext>> path);

        IIncludableRepository<TEntity, TNext> ThenWithMany<TNext>(
            Expression<Func<TProperty, IEnumerable<TNext>>> path);
    }
}
