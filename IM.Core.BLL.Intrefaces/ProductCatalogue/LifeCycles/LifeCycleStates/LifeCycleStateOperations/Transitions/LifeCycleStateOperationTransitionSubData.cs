using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations.Transitions;
public class LifeCycleStateOperationTransitionSubData
{
    public Guid ID { get; init; }

    public Guid OperationID { get; init; }

    public Guid FinishStateID { get; init; }

    public LifeCycleTransitionMode Mode { get; init; }
}
