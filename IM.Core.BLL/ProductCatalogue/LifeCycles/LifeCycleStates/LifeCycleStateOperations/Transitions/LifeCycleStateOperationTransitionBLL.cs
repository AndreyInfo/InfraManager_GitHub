using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.Extensions.Logging;
using System;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations.Transitions;

internal sealed class LifeCycleStateOperationTransitionBLL : StandardBLL<Guid, LifeCycleStateOperationTransition, LifeCycleStateOperationTransitionData, LifeCycleStateOperationTransitionDetails, LifeCycleStateOperationTransitionFilter>
    , ILifeCycleStateOperationTransitionBLL
    , ISelfRegisteredService<ILifeCycleStateOperationTransitionBLL>
{
    public LifeCycleStateOperationTransitionBLL(IRepository<LifeCycleStateOperationTransition> repository
        , ILogger<LifeCycleStateOperationTransitionBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<LifeCycleStateOperationTransitionDetails, LifeCycleStateOperationTransition> detailsBuilder
        , IInsertEntityBLL<LifeCycleStateOperationTransition, LifeCycleStateOperationTransitionData> insertEntityBLL
        , IModifyEntityBLL<Guid, LifeCycleStateOperationTransition, LifeCycleStateOperationTransitionData, LifeCycleStateOperationTransitionDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, LifeCycleStateOperationTransition> removeEntityBLL
        , IGetEntityBLL<Guid, LifeCycleStateOperationTransition, LifeCycleStateOperationTransitionDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, LifeCycleStateOperationTransition, LifeCycleStateOperationTransitionDetails, LifeCycleStateOperationTransitionFilter> detailsArrayBLL)
        : base(repository
            , logger
            , unitOfWork
            , currentUser
            , detailsBuilder
            , insertEntityBLL
            , modifyEntityBLL
            , removeEntityBLL
            , detailsBLL
            , detailsArrayBLL)
    {
    }
}
