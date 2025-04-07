using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System.Linq;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;
internal sealed class LifeCycleQueryBuilder :
    IBuildEntityQuery<LifeCycle, LifeCycleDetails, LifeCycleFilter>,
    ISelfRegisteredService<IBuildEntityQuery<LifeCycle, LifeCycleDetails, LifeCycleFilter>>
{
    private readonly IReadonlyRepository<LifeCycle> _repository;

    public LifeCycleQueryBuilder(IReadonlyRepository<LifeCycle> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<LifeCycle> Query(LifeCycleFilter filterBy)
    {
        var query = _repository.DisableTrackingForQuery().Query();

        if (!string.IsNullOrWhiteSpace(filterBy.SearchName))
            query = query.Where(c => c.Name.ToLower().Contains(filterBy.SearchName.ToLower()));

        if (filterBy.Types is not null && filterBy.Types.Any())
            query = query.Where(c => filterBy.Types.Contains(c.Type));

        return query;
    }
}
