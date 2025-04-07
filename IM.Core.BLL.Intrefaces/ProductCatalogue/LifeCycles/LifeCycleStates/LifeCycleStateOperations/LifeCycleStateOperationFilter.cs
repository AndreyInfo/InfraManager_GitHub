using System;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;

public class LifeCycleStateOperationFilter
{
    public Guid? LifeCycleStateID { get; init; }
    public string SearchName { get; init; }
    public Guid? RoleID { get; init; }
}