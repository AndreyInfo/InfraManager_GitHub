using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

internal abstract class LifeCycleStateLookupQueryBase
{
    private readonly byte? _lifeCycleType;
    private readonly DbSet<LifeCycleState> _lifeCycleStates;

    protected LifeCycleStateLookupQueryBase(DbSet<LifeCycleState> lifeCycleStates, byte? lifeCycleType)
    {
        _lifeCycleStates = lifeCycleStates;
        _lifeCycleType = lifeCycleType;
    }

    public virtual async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Array.ConvertAll(
            await _lifeCycleStates
                .Include(state => state.LifeCycle)
                .AsNoTracking()
                .Where(state => !state.LifeCycle.Removed) // todo: Move to FilterQuery ef core ?
                .Where(state => !_lifeCycleType.HasValue || state.LifeCycle.Type == (LifeCycleType)_lifeCycleType.Value)
                .Select(state => new
                {
                    ID = state.ID,
                    LifeCycleName = state.LifeCycle.Name,
                    LifeCycleStateName = state.Name,
                }).ToArrayAsync(cancellationToken),
            item => new ValueData
            {
                ID = item.ID.ToString(),
                Info = $"{item.LifeCycleName} \\ {item.LifeCycleStateName}",
            });
    }
}