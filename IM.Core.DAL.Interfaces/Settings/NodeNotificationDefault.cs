namespace InfraManager.DAL.Settings;

public class NodeNotificationDefault<TID> where TID : struct
{
    public TID ID { get; init; }

    public string Name { get; init; }

    public int? ParentID { get; init; }

    public bool HasChild { get; init; }

    public bool IsSelectFull { get; init; }

    public bool IsSelectPart { get; init; }
}
