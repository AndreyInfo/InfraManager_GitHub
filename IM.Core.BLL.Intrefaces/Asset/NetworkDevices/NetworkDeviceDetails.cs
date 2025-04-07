using System;

namespace InfraManager.BLL.Asset.NetworkDevices;

public class NetworkDeviceDetails
{
    public int ID { get; init; }
    public Guid IMObjID { get; init; }
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
    public string BuildingName { get; init; }
    public string FloorName { get; init; }
    public int RoomID { get; init; }
    public string RoomName { get; init; }
    public int? RackID { get; init; }
    public string RackName { get; init; }
    public int? RackPosition { get; init; } // Нет сущности RackViewObject
    public string Bios { get; init; }
    public string BiosVer { get; init; }
    public bool? CpuAutoInfo { get; init; }
    public string Cpu { get; init; }
    public float? CpuClockFrequency { get; init; }
    public int? CpuCoreNumber { get; init; }
    public Guid? CpuModelID { get; init; }
    public int? CpuNumber { get; init; }
    public string CsFormFactor { get; init; }
    public int? CsVendorID { get; init; }
    public string CsVendorName { get; init; }
    public string CsModel { get; init; }
    public string CsSize { get; init; }
    public DateTime? DateInquiry { get; init; } // После реализации механизма опроса
    public string DefaultPrinter { get; init; }
    public string Description { get; init; }
    public bool? DiskAutoInfo { get; init; }
    public float? DiskSpace { get; init; }
    public float? DiskSpaceTotal { get; init; }
    public Guid? DiskTypeID { get; init; }
    public string DiskTypeName { get; init; } // Название типа жесткого диска Каталога продуктов
    public Guid? LifeCycleStateID { get; init; }
    public string LifeCycleStateName { get; init; }
    public string LogicalLocation { get; init; }
    public string ExternalID { get; init; }
    public string ManufacturerName { get; init; }
    public float? Memory { get; init; }
    public string Note { get; init; }
    public bool OnStore { get; init; }
    public decimal? PowerConsumption { get; init; }
    public string ProductCatalogModelFullName { get; set; }
    public string ProductCatalogModelCode { get; init; }
    public bool? RamAutoInfo { get; init; }
    public float? RamSpace { get; init; }
    public string SNMPSecurityParametersName { get; init; } // После реализации SNMPSecurityParameters (Параметры подключения SNMPv3)
    public ObjectClass? UtilizerClassID { get; init; }
    public Guid? UtilizerID { get; init; }
    public Guid? CriticalityID { get; init; }
    public string CriticalityName { get; init; }
    public Guid? InfrastructureSegmentID { get; init; }
    public string InfrastructureSegmentName { get; init; }
    public int? NetworkDeviceModelID { get; init; }
    public bool IsWorking { get; init; }
    public Guid? NetworkNodeID { get; set; }
    public string NetworkNodeName { get; set; }
    public string Location { get; set; }
    public Guid? OwnerID { get; init; }
    public ObjectClass? OwnerClassID { get; init; }
    public DateTime? DateReceived { get; init; }

    public int? PortCount { get; init; }
    public int SlotCount { get; init; }
    public decimal? HeightInUnits { get; init; }
    public decimal Height { get; init; }
    public decimal Width { get; init; }
    public decimal? Depth { get; init; }
    public bool IsRackmount { get; init; }
    public Guid? SnmpTokenID { get; init; }
}