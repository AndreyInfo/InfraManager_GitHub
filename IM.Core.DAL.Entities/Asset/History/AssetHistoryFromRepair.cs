using System;

namespace InfraManager.DAL.Asset.History;
public class AssetHistoryFromRepair
{
    public Guid ID { get; init; }
    public string RepairType { get; init; }
    public float Cost { get; init; }
    public string Quality { get; init; }
    public string Agreement { get; init; }
    public string ReasonNumber { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }

    public virtual AssetHistory AssetHistory { get; }

}
