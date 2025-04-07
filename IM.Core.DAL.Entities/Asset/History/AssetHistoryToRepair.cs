using System;

namespace InfraManager.DAL.Asset.History;
public class AssetHistoryToRepair
{
    public Guid ID { get; init; }
    public ObjectClass? LocationClassID { get; init; }
    public Guid? LocationID { get; init; }
    public DateTime? UtcDateAnticipated { get; init; }
    public Guid? ServiceCenterID { get; init; }
    public string ServiceCenterName { get; set; }
    public Guid? ServiceContractID { get; init; }
    public string ServiceContractNumber { get; set; }
    public string Problems { get; init; }
    public string ReasonNumber { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }

    public virtual AssetHistory AssetHistory { get; }
}
