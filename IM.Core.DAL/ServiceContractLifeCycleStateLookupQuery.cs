using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

internal class ServiceContractLifeCycleStateLookupQuery : LifeCycleStateLookupQueryBase, ILookupQuery
{
    public ServiceContractLifeCycleStateLookupQuery(DbSet<LifeCycleState> lifeCycleStates)
        : base(lifeCycleStates, lifeCycleType: 3) // todo: What a magic number? Please help!
    {
    }
}