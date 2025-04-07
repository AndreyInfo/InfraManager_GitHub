using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.AssetsManagement.Hardware;

internal class HardwareStateLookupQuery : LifeCycleStateLookupQueryBase, ILookupQuery
{
    public HardwareStateLookupQuery(DbSet<LifeCycleState> lifeCycleStates)
        : base(lifeCycleStates, lifeCycleType: 0) // todo: What a magic number? Please help!
    {
    }
}