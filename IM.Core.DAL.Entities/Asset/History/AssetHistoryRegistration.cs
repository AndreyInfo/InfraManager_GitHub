using System;

namespace InfraManager.DAL.Asset.History;
public class AssetHistoryRegistration
{
    public Guid ID { get; init; }
    public ObjectClass NewLocationClassID { get; init; }
    public Guid NewLocationID { get; init; }
    public string NewLocationName { get; set; }
    public ObjectClass? OwnerClassID { get; init; }
    public Guid? OwnerID { get; init; }
    public string OwnerName { get; set; }
    public Guid? UserID { get; init; }
    public string UserFullName { get; init; }
    public string Founding { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }
    public Guid? NewStorageLocationID { get; init; }
    public string NewStorageLocationName { get; init; }

    public virtual AssetHistory AssetHistory { get; }
}
