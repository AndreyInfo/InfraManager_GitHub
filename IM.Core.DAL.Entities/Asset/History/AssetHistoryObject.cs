using System;

namespace InfraManager.DAL.Asset.History;
public class AssetHistoryObject
{
    public AssetHistoryObject()
    {
        
    }
    public AssetHistoryObject(Guid assetHistoryID, Guid objectID, ObjectClass objectClassID, string objectName)
    {
        ID = assetHistoryID;
        ObjectID = objectID;
        ObjectClassID = objectClassID;
        ObjectName = objectName;
    }

    public Guid ID { get; init; }
    public ObjectClass ObjectClassID { get; init; }
    public Guid ObjectID { get; init; }
    public string ObjectName { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }

    public virtual AssetHistory AssetHistory { get; }
}
