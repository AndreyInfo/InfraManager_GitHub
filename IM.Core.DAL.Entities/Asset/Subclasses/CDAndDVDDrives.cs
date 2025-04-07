using Inframanager;
using System;

namespace InfraManager.DAL.Asset.Subclasses;

[ObjectClassMapping(ObjectClass.CDAndDVDDrives)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class CDAndDVDDrives : IAssetSubclass
{
    public Guid ID { get; init; }
    public string WriteSpeed { get; set; }
    public string ReadSpeed { get; set; }
    public string DriveCapabilities { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
}
