using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;
internal class LifeCycleStateOperationLoader : ILoadEntity<Guid, LifeCycleStateOperation>
    , ISelfRegisteredService<ILoadEntity<Guid, LifeCycleStateOperation>>
{
    private readonly IReadonlyRepository<LifeCycleStateOperation> _lifeCycleStates;

    public LifeCycleStateOperationLoader(IReadonlyRepository<LifeCycleStateOperation> lifeCycleStates)
    {
        _lifeCycleStates = lifeCycleStates;
    }

    public async Task<LifeCycleStateOperation> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _lifeCycleStates.WithMany(c => c.Transitions)
            .ThenWith(c => c.FinishState)
            .DisableTrackingForQuery()
            .FirstOrDefaultAsync(c => c.ID == id, cancellationToken);
    }
}
