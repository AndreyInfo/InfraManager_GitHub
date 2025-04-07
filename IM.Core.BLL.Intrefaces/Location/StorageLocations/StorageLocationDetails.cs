using System;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.StorageLocations;

public class StorageLocationDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string ExternalID { get; init; }
    public Guid? UserID { get; init; }
    public byte[] RowVersion { get; init; }
    public string MOL { get; init; }

    public StorageLocationReference[] StorageLocationReferences { get; init; }
}
