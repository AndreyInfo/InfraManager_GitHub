using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.DAL
{
    public interface IIncludableGlobalIdentifiedEntityFinder<TEntity, TProperty> : 
        IFindEntityByGlobalIdentifier<TEntity>
        where TEntity : class, IGloballyIdentifiedEntity
    {
        IIncludableGlobalIdentifiedEntityFinder<TEntity, TNext> ThenWith<TNext>(
            Expression<Func<TProperty, TNext>> path);

        IIncludableGlobalIdentifiedEntityFinder<TEntity, TNext> ThenWithMany<TNext>(
            Expression<Func<TProperty, IEnumerable<TNext>>> path);
    }
}
