using System;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations.Transitions;
public class LifeCycleStateOperationTransitionFilter
{
    public Guid? OperationID { get; init; }
    public Guid? StateID { get; init; }
}
