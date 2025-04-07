using InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations.Transitions;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;

public class LifeCycleStateOperationData
{
    public string Name { get; init; }
    public int Sequence { get; init; }
    public LifeCycleOperationCommandType CommandType { get; init; }
    public Guid? WorkOrderTemplateID { get; init; }
    public Guid LifeCycleStateID { get; init; }
    public string IconName { get; init; }
    public LifeCycleStateOperationTransitionSubData[] Transitions { get; init; }
}
