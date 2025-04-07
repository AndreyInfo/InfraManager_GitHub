using System;

namespace InfraManager.DAL.Asset.History;
public class AssetHistoryMove
{
    public AssetHistoryMove()
    {
        
    }
    public AssetHistoryMove(Guid id)
    {
        ID = id;
    }

    public Guid ID { get; init; }
    public ObjectClass NewLocationClassID { get; init; }
    public Guid NewLocationID { get; init; }
    public string NewLocationName { get; set; }
    public string ReasonNumber { get; init; }
    public ObjectClass? UtilizerClassID { get; init; }
    public Guid? UtilizerID { get; init; }
    public string UtilizerName { get; set; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }

    public virtual AssetHistory AssetHistory { get; }
}
