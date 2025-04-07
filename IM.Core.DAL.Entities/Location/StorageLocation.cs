using Inframanager;
using System;
using System.Collections.Generic;


namespace InfraManager.DAL.Location;

[ObjectClassMapping(ObjectClass.StorageLocation)]
[OperationIdMapping(ObjectAction.Insert, OperationID.StorageLocation_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.StorageLocation_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.StorageLocation_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.StorageLocation_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.StorageLocation_Properties)]
public class StorageLocation : Catalog<Guid>
{
    public StorageLocation()
    { }

    public StorageLocation(string name, string externalID, Guid? userID)
    {
        ID = Guid.NewGuid();
        Name = name;
        ExternalID = externalID;
        UserID = userID;
    }

    public string ExternalID { get; init; }
    public Guid? UserID { get; init; }
    public byte[] RowVersion { get; init; }

    public virtual User User { get; init; }
    public virtual ICollection<StorageLocationReference> StorageLocationReferences{ get; init; }

}
