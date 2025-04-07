using System;

namespace InfraManager.BLL.Location.StorageLocations;

public class LocationListItem
{
    public string Location { get; init; }

    public Guid ID { get; init; }

    public ObjectClass ClassID { get; init; } 
}
