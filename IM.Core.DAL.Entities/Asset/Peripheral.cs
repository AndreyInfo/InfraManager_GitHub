using Inframanager;
using System;
using InfraManager.DAL.Location;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.Peripherial)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Peripheral_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Peripheral_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Peripheral_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Peripheral_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class Peripheral : IProduct<PeripheralType>, IHardwareAsset
{
    public Guid IMObjID { get; init; }
    public Guid? PeripheralTypeID { get; set; }
    public int? TerminalDeviceID { get; set; }
    public int? NetworkDeviceID { get; set; }
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public string Note { get; set; }
    public int ID { get; init; }
    public int StateID { get; set; }
    public int RoomID { get; set; }
    public decimal? BWLoad { get; set; }
    public decimal? ColorLoad { get; set; }
    public decimal? PhotoLoad { get; set; }
    public ObjectClass? ClassID { get; private set; }
    public byte[] RowVersion { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
    public int? ComplementaryIntID { get; set; }
    public string Code { get; set; }
    public string ExternalID { get; set; }

    public virtual PeripheralType Model { get; set; }
    public virtual TerminalDevice TerminalDevice { get; init; }
    public virtual NetworkDevice NetworkDevice { get; init; }
    public virtual Room Room { get; init; }
}