using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations.Transitions;
internal sealed class LifeCycleStateOperationTransitionQueryBuilder :
    IBuildEntityQuery<LifeCycleStateOperationTransition, LifeCycleStateOperationTransitionDetails, LifeCycleStateOperationTransitionFilter>,
    ISelfRegisteredService<IBuildEntityQuery<LifeCycleStateOperationTransition, LifeCycleStateOperationTransitionDetails, LifeCycleStateOperationTransitionFilter>>
{

    private readonly IReadonlyRepository<LifeCycleStateOperationTransition> _repository;

    public LifeCycleStateOperationTransitionQueryBuilder(IReadonlyRepository<LifeCycleStateOperationTransition> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<LifeCycleStateOperationTransition> Query(LifeCycleStateOperationTransitionFilter filterBy)
    {
        var query = _repository.Query();

        if (filterBy.OperationID.HasValue)
            query = query.Where(c => c.OperationID == filterBy.OperationID);

        if (filterBy.StateID.HasValue)
            query = query.Where(c => c.FinishStateID == filterBy.StateID);

        return query;
    }
}
