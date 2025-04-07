using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;

namespace InfraManager.BLL.Asset.History;
public class AssetHistoryBaseData
{
    public ObjectClass ClassID { get; init; }
    public LifeCycleOperationCommandType OperationType { get; set; }
    public Guid? LocationID { get; init; }
    public ObjectClass? LocationClassID { get; init; }
    public Guid? LifeCycleStateID { get; set; }
    public string ReasonNumber { get; init; }
    public ObjectClass? OwnerClassID { get; init; }
    public Guid? OwnerID { get; init; }
    public ObjectClass? UtilizerClassID { get; init; }
    public Guid? UtilizerID { get; init; }
    public DateTime? UtcDateAnticipated { get; init; }
    public Guid? ServiceCenterID { get; init; }
    public Guid? ServiceContractID { get; init; }
    public string Problems { get; init; }
    public string RepairType { get; init; }
    public float Cost { get; init; }
    public string Quality { get; init; }
    public string Agreement { get; init; }
}
