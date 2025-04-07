using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;

namespace InfraManager.DAL.Asset.History;
public class AssetHistoryChangeAssetState
{
    public AssetHistoryChangeAssetState()
    {
        
    }

    public AssetHistoryChangeAssetState(Guid id, Guid? lifeCycleStateID, string lifeCycleStateName)
    {
        ID = id;
        LifeCycleStateID = lifeCycleStateID;
        LifeCycleStateName = lifeCycleStateName;
    }

    public Guid ID { get; init; }
    public string ReasonNumber { get; init; }
    public Guid? LifeCycleStateID { get; init; }
    public string LifeCycleStateName { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }

    public virtual AssetHistory AssetHistory { get; }
    public virtual LifeCycleState LifeCycleState { get; }
}
