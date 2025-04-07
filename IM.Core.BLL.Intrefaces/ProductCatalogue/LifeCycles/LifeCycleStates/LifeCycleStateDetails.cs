using InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;

public class LifeCycleStateDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public Guid LifeCycleID { get; init; }
    public bool IsDefault { get; init; }
    public LifeCycleOperationCommandType[] Commands { get; init; }
    public LifeCycleStateOptions Options { get; init; }
    public LifeCycleStateOperationDetails[] Operations { get; init; }
}
