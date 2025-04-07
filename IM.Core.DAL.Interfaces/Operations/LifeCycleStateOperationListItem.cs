using System;
using System.Data;

namespace InfraManager.DAL.Operations;

public class LifeCycleStateOperationListItem
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public bool IsSelected { get; init; }
    public int? CommandTypeID { get; set; }
}