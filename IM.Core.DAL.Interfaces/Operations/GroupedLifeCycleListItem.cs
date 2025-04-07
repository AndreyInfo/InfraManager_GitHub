namespace InfraManager.DAL.Operations;

public class GroupedLifeCycleListItem
{
    public string Name { get; init; }
    public LifeCycleStateListItem[] States { get; init; }
}