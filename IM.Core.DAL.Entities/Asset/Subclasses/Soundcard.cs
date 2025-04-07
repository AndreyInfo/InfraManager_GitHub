using Inframanager;
using System;

namespace InfraManager.DAL.Asset.Subclasses;

[ObjectClassMapping(ObjectClass.Soundcard)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class Soundcard : IAssetSubclass
{
    public Guid ID { get; init; }
    public string DMa { get; set; }
    public string IRq { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
}
