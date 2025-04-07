using System;

namespace InfraManager.BLL.Asset.NetworkDevices;

public class NetworkDeviceData
{
    public string Name { get; init; }
    public string IpAddress { get; init; }
    public string IpMask { get; init; }
    public string InvNumber { get; init; }
    public string SerialNumber { get; init; }
    public string AssetTag { get; init; }
    public string Identifier { get; init; }
    public string Code { get; init; }
    public Guid? OrganizationItemID { get; init; }
    public int? OrganizationItemClassID { get; init; }
    public int RoomID { get; init; }
    public int? RackID { get; init; }
    public string Bios { get; init; }
    public string BiosVer { get; init; }
    public bool? CpuAutoInfo { get; init; }
    public float? CpuClockFrequency { get; init; }
    public int? CpuCoreNumber { get; init; }
    public Guid? CpuModelID { get; init; }
    public string Cpu { get; init; }
    public int? CpuNumber { get; init; }
    public string CsFormFactor { get; init; }
    public int? CsVendorID { get; init; }
    public string CsModel { get; init; }
    public string CsSize { get; init; }
    public string DefaultPrinter { get; init; }
    public string Description { get; init; }
    public bool? DiskAutoInfo { get; init; }
    public float? DiskSpace { get; init; }
    public float? DiskSpaceTotal { get; init; }
    public Guid? DiskTypeID { get; init; }
    public float? Memory { get; init; }
    public string Note { get; init; }
    public bool OnStore { get; init; }
    public decimal? PowerConsumption { get; init; }
    public bool? RamAutoInfo { get; init; }
    public float? RamSpace { get; init; }
    public ObjectClass? UtilizerClassID { get; init; }
    public Guid? UtilizerID { get; init; }
    public Guid? CriticalityID { get; init; }
    public Guid? InfrastructureSegmentID { get; init; }
    public int? NetworkDeviceModelID { get; init; }
    public bool IsWorking { get; init; }
    public Guid? LifeCycleStateID { get; init; }
    public Guid? OwnerID { get; init; }
    public ObjectClass? OwnerClassID { get; init; }
    public Guid? SnmpTokenID { get; init; }
}
