using Inframanager;
using InfraManager.DAL.Location;
using System;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.TerminalDevice)]
[OperationIdMapping(ObjectAction.Insert, OperationID.TerminalDevice_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.TerminalDevice_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.TerminalDeviceModel_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.TerminalDevice_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.TerminalDevice_Properties)]
public partial class TerminalDevice : INamedEntity
    , IGloballyIdentifiedEntity
    , IMarkableForDelete
    , IProduct<TerminalDeviceModel>
    , IHardwareAsset
{
    public int ID { get; init; }
    public string Name { get; set; }
    public int? TypeID { get; set; }
    public int? UserID { get; set; }
    public string IpAddress { get; set; }
    public string IpMask { get; set; }
    public int? RoomID { get; set; }
    public string Note { get; set; }
    public string MacAddress { get; set; }
    public int Connected { get; set; }
    public string Connection1 { get; set; }
    public int? VisioID { get; set; }
    public bool Removed { get; protected set; }
    public void MarkForDelete()
    {
        Removed = true;
    }

    public DateTime? DateRemoved { get; set; }
    public string InvNumber { get; set; }
    public string Cpu { get; set; }
    public string ClockFrequency { get; set; }
    public string Bios { get; set; }
    public string Bus { get; set; }
    public long? Memory { get; set; }
    public string VideoAdapter { get; set; }
    public string Keyboard { get; set; }
    public string Mouse { get; set; }
    public string SerialPorts { get; set; }
    public string ParallelPorts { get; set; }
    public string OperatingSystem { get; set; }
    public string DefaultPrinter { get; set; }
    public string ExternalID { get; set; }
    public string FrameType { get; set; }
    public string AssetTag { get; set; }
    public string Cpusocket { get; set; }
    public bool? Usbexist { get; set; }
    public string Monitor { get; set; }
    public string MonitorResolution { get; set; }
    public string Osver { get; set; }
    public string SerialNumber { get; set; }
    public string Biosver { get; set; }
    public int? VideoMemory { get; set; }
    public string Connection { get; set; }
    public int? CsVendorID { get; set; }
    public string CsModel { get; set; }
    public string CsSize { get; set; }
    public string CsFormFactor { get; set; }
    public int? MbVendorID { get; set; }
    public string MbModel { get; set; }
    public string MbChipSet { get; set; }
    public string MbSlots { get; set; }
    public int? ConnectorID { get; set; }
    public int? TechnologyID { get; set; }
    public string Code { get; set; }
    public int? ConnectedPortID { get; set; }
    public Guid IMObjID { get; init; }
    public decimal? PowerConsumption { get; set; }
    public byte[] RowVersion { get; init; }
    public Guid? PeripheralDatabaseID { get; set; }
    public int? ComplementaryID { get; set; }
    public Guid? ComplementaryGuidID { get; set; }
    public string LogicalLocation { get; set; }
    public string Description { get; set; }
    public string Identifier { get; set; }
    public Guid? SnmpTokenID { get; set; }
    public int? CpucoreNumber { get; set; }
    public int? Cpunumber { get; set; }
    public Guid? Cpumodel { get; set; }
    public Guid? DiskType { get; set; }
    public bool? CpuautoInfo { get; set; }
    public bool? DiskAutoInfo { get; set; }
    public bool? RamautoInfo { get; set; }
    public float? DiskSpaceTotal { get; set; }
    public float? Ramspace { get; set; }
    public float? CpuclockFrequency { get; set; }
    public Guid? InfrastructureSegmentID { get; set; }

    public virtual Workplace Workplace { get; set; }
    public virtual TerminalDeviceModel Model { get; set; }
    public virtual Room Room { get; init; }

    public string GetName()
    {
        return Name;
    }
}