using System.Collections.Generic;

namespace InfraManager.DAL.Operations;

public class GroupedOperationListItem
{
    public string Name { get; init; }
    public IEnumerable<OperationListItem> Operations { get; init; } 

}