using Inframanager;
using System;

namespace InfraManager.DAL.Asset.Subclasses;

[ObjectClassMapping(ObjectClass.Processor)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public partial class Processor : IAssetSubclass
{
    public Guid ID { get; init; }
    public string MaxClockSpeed { get; set; }
    public string CurrentClockSpeed { get; set; }
    public string L2cacheSize { get; set; }
    public string SocketDesignation { get; set; }
    public string Platform { get; set; }
    public string NumberOfCores { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
}