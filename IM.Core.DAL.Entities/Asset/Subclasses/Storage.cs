using Inframanager;
using System;

namespace InfraManager.DAL.Asset.Subclasses;

[ObjectClassMapping(ObjectClass.Storage)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class Storage : IAssetSubclass
{
    public Guid ID { get; init; }
    public string FormattedCapacity { get; set; }
    public string RecordingSurfaces { get; set; }
    public string InterfaceType { get; set; }
    public int? StorageClassID { get; set; }
    public Guid? StorageID { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
}
