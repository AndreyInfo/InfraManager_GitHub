using System;
using Inframanager;
using InfraManager.DAL.Asset.History;
using InfraManager.DAL.Location;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.ActiveDevice)]
[OperationIdMapping(ObjectAction.Insert, OperationID.NetworkDevice_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.NetworkDevice_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.NetworkDevice_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.NetworkDevice_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.NetworkDevice_Properties)]
public partial class NetworkDevice : IGloballyIdentifiedEntity, IMarkableForDelete, IProduct<NetworkDeviceModel>, IHardwareAsset, ILocationObject, IHistoryNamedEntity
{
    public int ID { get; init; }
    public string Name { get; set; }
    public int? NetworkDeviceModelID { get; init; }
    public int? RackID { get; set; }
    public string IpAddress { get; init; }
    public string IpMask { get; init; }
    public string Note { get; set; }
    public int Connected { get; init; }
    public int? VisioID { get; init; }
    public bool Removed { get; protected set; }
    public void MarkForDelete()
    {
        Removed = true;
    }

    public DateTime? DateRemoved { get; init; }
    public string InvNumber { get; set; }
    public string Cpu { get; init; }
    public string ClockFrequency { get; init; }
    public string Bios { get; init; }
    public string Bus { get; init; }
    public long? MemorySize { get; init; }
    public string VideoAdapter { get; init; }
    public string Keyboard { get; init; }
    public string Mouse { get; init; }
    public string SerialPorts { get; init; }
    public string ParallelPorts { get; init; }
    public string OperatingSystem { get; init; }
    public string DefaultPrinter { get; init; }
    public string ExternalID { get; set; }
    public string FrameType { get; init; }
    public string AssetTag { get; set; }
    public string CpuSocket { get; init; }
    public bool? UsbExist { get; init; }
    public string Monitor { get; init; }
    public string MonitorResolution { get; init; }
    public string OSVer { get; init; }
    public string SerialNumber { get; set; }
    public string BiosVer { get; init; }
    public int? VideoMemory { get; init; }
    public int? CsVendorID { get; init; }
    public string CsModel { get; init; }
    public string CsSize { get; init; }
    public string CsFormFactor { get; init; }
    public int? MbVendorID { get; init; }
    public string MbModel { get; init; }
    public string MbChipSet { get; init; }
    public string MbSlots { get; init; }
    public int RoomID { get; set; }
    public string Code { get; set; }
    public byte[] RowVersion { get; init; }
    public Guid IMObjID { get; init; }
    public decimal? PowerConsumption { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public int? ComplementaryID { get; init; }
    public Guid? ComplementaryGuidID { get; init; }
    public string LogicalLocation { get; init; }
    public string Description { get; init; }
    public string Identifier { get; init; }
    public Guid? SnmpTokenID { get; init; }
    public Guid? InfrastructureSegmentID { get; set; }
    public Guid? CriticalityID { get; set; }
    public float? Memory { get; init; }
    public float? DiskSpace { get; init; }
    public Guid? OrganizationItemID { get; init; }
    public int? OrganizationItemClassID { get; init; }
    public int? CpuCoreNumber { get; init; }
    public int? CpuNumber { get; init; }
    public Guid? CpuModelID { get; init; }
    public Guid? DiskTypeID { get; init; }
    public bool? CpuAutoInfo { get; init; }
    public bool? DiskAutoInfo { get; init; }
    public bool? RamAutoInfo { get; init; }
    public float? DiskSpaceTotal { get; init; }
    public float? RamSpace { get; init; }
    public float? CpuClockFrequency { get; init; }

    public virtual NetworkDeviceModel Model { get; set; }
    public virtual Criticality Criticality { get; init; }
    public virtual InfrastructureSegment InfrastructureSegment { get; init; }
    public virtual Rack Rack { get; init; }
    public virtual Room Room { get; init; }

    public string GetFullPath()
    {
        var roomLocation = Room is null ? "" : Room.GetFullPath();
        var rackLocation = Rack is null ? "" : Rack.GetFullPath();
        return roomLocation + rackLocation;
    }

    public string GetObjectName()
        => $"{Name} {SerialNumber} {InvNumber}";
}