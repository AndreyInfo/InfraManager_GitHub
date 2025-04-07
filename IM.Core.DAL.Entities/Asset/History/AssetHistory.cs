using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;

namespace InfraManager.DAL.Asset.History;
public class AssetHistory
{
    public AssetHistory()
    {
        ID = Guid.NewGuid();
        UtcDate = DateTime.UtcNow;
    }

    public Guid ID { get; init; }
    public DateTime UtcDate { get; init; }
    public Guid UserID { get; init; }
    public string UserFullName { get; init; }
    public LifeCycleOperationCommandType OperationType { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }
}
