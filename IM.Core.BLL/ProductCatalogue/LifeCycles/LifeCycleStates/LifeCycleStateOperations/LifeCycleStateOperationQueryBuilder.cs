using Inframanager.BLL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;
internal sealed class LifeCycleStateOperationQueryBuilder :
    IBuildEntityQuery<LifeCycleStateOperation, LifeCycleStateOperationDetails, LifeCycleStateOperationFilter>,
    ISelfRegisteredService<IBuildEntityQuery<LifeCycleStateOperation, LifeCycleStateOperationDetails, LifeCycleStateOperationFilter>>
{
    private readonly IReadonlyRepository<LifeCycleStateOperation> _repository;

    public LifeCycleStateOperationQueryBuilder(IReadonlyRepository<LifeCycleStateOperation> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<LifeCycleStateOperation> Query(LifeCycleStateOperationFilter filterBy)
    {
        var query = _repository.Query();

        if (filterBy.LifeCycleStateID.HasValue)
            query = query.Where(c => c.LifeCycleStateID == filterBy.LifeCycleStateID);

        if (!string.IsNullOrWhiteSpace(filterBy.SearchName))
            query = query.Where(c => c.Name.ToLower().Contains(filterBy.SearchName.ToLower()));

        return query;
    }
}
