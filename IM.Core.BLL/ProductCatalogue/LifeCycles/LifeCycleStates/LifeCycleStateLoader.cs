using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;
internal sealed class LifeCycleStateLoader : ILoadEntity<Guid, LifeCycleState>
    , ISelfRegisteredService<ILoadEntity<Guid, LifeCycleState>>
{
    private readonly IReadonlyRepository<LifeCycleState> _lifeCycleStates;

    public LifeCycleStateLoader(IReadonlyRepository<LifeCycleState> lifeCycleStates)
    {
        _lifeCycleStates = lifeCycleStates;
    }

    public async Task<LifeCycleState> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _lifeCycleStates.WithMany(c => c.LifeCycleStateOperations)
            .ThenWithMany(c => c.Transitions)
            .ThenWith(c => c.FinishState)
            .DisableTrackingForQuery()
            .FirstOrDefaultAsync(c => c.ID == id, cancellationToken);
    }
}