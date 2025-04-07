using System;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.StorageLocations;

public class StorageLocationInsertDetails
{
    public string Name { get; init; }
    public string ExternalID { get; init; }
    public Guid? UserID { get; init; }

    public StorageLocationReference[] StorageLocationReferences { get; init; }
}
