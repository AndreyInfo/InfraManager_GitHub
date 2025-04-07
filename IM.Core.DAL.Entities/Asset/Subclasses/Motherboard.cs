using Inframanager;
using System;

namespace InfraManager.DAL.Asset.Subclasses;

[ObjectClassMapping(ObjectClass.Motherboard)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class Motherboard : IAssetSubclass
{
    public Guid ID { get; init; }
    public string PrimaryBusType { get; set; }
    public string SecondaryBusType { get; set; }
    public string ExpansionSlots { get; set; }
    public string RamSlots { get; set; }
    public string MotherboardSize { get; set; }
    public string MotherboardChipset { get; set; }
    public string MaximumSpeed { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
}
