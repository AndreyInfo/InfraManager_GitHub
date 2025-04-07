using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public interface IFindEntityByGlobalIdentifier<TEntity> : IFindEntityByGlobalIdentifier
        where TEntity : class, IGloballyIdentifiedEntity
    {
        TEntity Find(Guid globalID);
        Task<TEntity> FindAsync(Guid globalId, CancellationToken token = default);
        IIncludableGlobalIdentifiedEntityFinder<TEntity, TNext> With<TNext>(
            Expression<Func<TEntity, TNext>> path);
        IIncludableGlobalIdentifiedEntityFinder<TEntity, TNext> WithMany<TNext>(
            Expression<Func<TEntity, IEnumerable<TNext>>> path);
    }

    public interface IFindEntityByGlobalIdentifier
    {
        Task<IGloballyIdentifiedEntity> FindAsync(Guid globalID, CancellationToken token = default);
    }
}
