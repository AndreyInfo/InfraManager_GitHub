using Inframanager;
using System;

namespace InfraManager.DAL.Asset;

[OperationIdMapping(ObjectAction.Insert, OperationID.FileSystem_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.FileSystem_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.FileSystem_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.FileSystem_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.FileSystem_Properties)]
public class FileSystem : Catalog<Guid>
{
    public byte[] RowVersion { get; set; }
    public Guid? ComplementaryID { get; set; }
}
