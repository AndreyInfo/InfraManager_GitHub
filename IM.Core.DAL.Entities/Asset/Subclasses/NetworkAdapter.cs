using Inframanager;
using System;

namespace InfraManager.DAL.Asset.Subclasses;

[ObjectClassMapping(ObjectClass.NetworkAdapter)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class NetworkAdapter : IAssetSubclass
{
    public Guid ID { get; init; }
    public string MaxSpeed { get; set; }
    public string InterfaceType { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
}
