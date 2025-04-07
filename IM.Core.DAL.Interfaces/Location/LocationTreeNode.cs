using System;

namespace InfraManager.DAL.Location;

public class LocationTreeNode
{
    public int ID { get; init; }

    public ObjectClass ClassID { get; init; }

    public string Name { get; init; }

    public Guid UID { get; init; }

    public int? ParentID { get; init; }

    public Guid? ParentUID { get; init; }

    public bool HasChild { get; init; }

    public string IconName { get; init; }

    public bool IsSelectPart { get; init; }

    public bool IsSelectFull { get; init; }
}
