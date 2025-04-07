using System;

namespace InfraManager.BLL.Location;

public class LocationTreeNodeDetails
{
    public int ID { get; init; }

    public ObjectClass ClassID { get; init; }

    public string Name { get; init; }

    public Guid UID { get; init; }

    public int? ParentID { get; init; }
    
    public Guid? ParentUID { get; init; }

    public bool HasChild { get; set; }

    public string IconName { get; set; }
    
    public bool IsSelectPart { get; set; }

    public bool IsSelectFull { get; set; }
}
