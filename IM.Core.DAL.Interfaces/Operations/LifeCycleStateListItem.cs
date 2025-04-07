using System;

namespace InfraManager.DAL.Operations;

public class LifeCycleStateListItem
{
    public string Name { get; init; }
    public Guid ID { get; init; }
    public LifeCycleStateOperationListItem[] Operations { get; init; }
}