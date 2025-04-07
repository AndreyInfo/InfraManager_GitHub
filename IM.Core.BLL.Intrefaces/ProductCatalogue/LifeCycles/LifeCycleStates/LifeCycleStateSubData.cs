using InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;
using System;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;
public class LifeCycleStateSubData
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public Guid LifeCycleID { get; init; }
    public bool IsDefault { get; init; }
    public LifeCycleStateOptions Options { get; init; }
    public LifeCycleStateOperationSubData[] Operations { get; init; }
}