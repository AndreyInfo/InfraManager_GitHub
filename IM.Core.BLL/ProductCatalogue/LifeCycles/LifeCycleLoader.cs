using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;
internal sealed class LifeCycleLoader : ILoadEntity<Guid, LifeCycle>
    , ISelfRegisteredService<ILoadEntity<Guid, LifeCycle>>
{
    private readonly IReadonlyRepository<LifeCycle> _lifeCycles;

    public LifeCycleLoader(IReadonlyRepository<LifeCycle> lifeCycles)
    {
        _lifeCycles = lifeCycles;
    }

    public async Task<LifeCycle> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _lifeCycles.WithMany(c => c.LifeCycleStates)
            .ThenWithMany(c => c.LifeCycleStateOperations)
            .ThenWithMany(c => c.Transitions)
            .ThenWith(c => c.FinishState)
            .DisableTrackingForQuery()
            .FirstOrDefaultAsync(c => c.ID == id, cancellationToken);
    }
}
