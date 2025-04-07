using System;

namespace InfraManager.DAL.Location;

public class StorageLocationReference
{
    public Guid StorageLocationID { get; set; }
    
    public Guid ObjectID { get; init; }
    
    public ObjectClass ObjectClassID { get; init; }
}
