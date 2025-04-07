using System;

namespace InfraManager.BLL.Location;

public class LocationTreeFilter
{
    public ObjectClass ClassID { get; init; }
    public int ParentID { get; set; }
    public Guid? OrganizationID { get; set; }
    public ObjectClass? ChildClassID { get; init; }
}
