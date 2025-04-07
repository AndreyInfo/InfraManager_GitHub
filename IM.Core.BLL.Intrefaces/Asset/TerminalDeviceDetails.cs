using System;

namespace InfraManager.BLL.Asset
{
    public class TerminalDeviceDetails
    {
        public int ID { get; init; }
        public string Name { get; init; }
        public int? TypeID { get; init; }
        public int? UserID { get; init; }
        public string IpAddress { get; init; }
        public string IpMask { get; init; }
        public int? RoomID { get; init; }
        public string Note { get; init; }
        public string MacAddress { get; init; }
        public int Connected { get; init; }
        public string Connection1 { get; init; }
        public int? VisioId { get; init; }
        public bool Removed { get; init; }
        public DateTime? DateRemoved { get; init; }
        public string InvNumber { get; init; }
        public string Cpu { get; init; }
        public string CPUFreq { get; init; }
        public string Bios { get; init; }
        public string Bus { get; init; }
        public long? RAM { get; init; }
        public string GPU { get; init; }
        public string Keyboard { get; init; }
        public string Mouse { get; init; }
        public string COMPorts { get; init; }
        public string LPTPorts { get; init; }
        public string OS { get; init; }
        public string DefaultPrinter { get; init; }
        public string ExternalId { get; init; }
        public string FrameType { get; init; }
        public string AssetTag { get; init; }
        public string Cpusocket { get; init; }
        public bool? Usbexist { get; init; }
        public string Monitor { get; init; }
        public string MonitorResolution { get; init; }
        public string Osver { get; init; }
        public string SerialNumber { get; init; }
        public string Biosver { get; init; }
        public int? VideoMemory { get; init; }
        public string Connection { get; init; }
        public int? CsVendorId { get; init; }
        public string CsModel { get; init; }
        public string CsSize { get; init; }
        public string CsFormFactor { get; init; }
        public int? MbVendorId { get; init; }
        public string MbModel { get; init; }
        public string MbChipSet { get; init; }
        public string MbSlots { get; init; }
        public int? ConnectorId { get; init; }
        public int? TechnologyId { get; init; }
        public string Code { get; init; }
        public int? ConnectedPortId { get; init; }
        public Guid IMObjID { get; init; }
        public decimal? PowerConsumption { get; init; }
        public byte[] RowVersion { get; init; }
        public Guid? PeripheralDatabaseId { get; init; }
        public int? ComplementaryId { get; init; }
        public Guid? ComplementaryGuidId { get; init; }
        public string LogicalLocation { get; init; }
        public string Description { get; init; }
        public string Identifier { get; init; }
        public Guid? SnmpTokenId { get; init; }
        public int? CpucoreNumber { get; init; }
        public int? Cpunumber { get; init; }
        public Guid? Cpumodel { get; init; }
        public Guid? DiskType { get; init; }
        public bool? CpuautoInfo { get; init; }
        public bool? DiskAutoInfo { get; init; }
        public bool? RamautoInfo { get; init; }
        public float? DiskSpaceTotal { get; init; }
        public float? Ramspace { get; init; }
        public float? CpuclockFrequency { get; init; }
        public Guid? InfrastructureSegmentId { get; init; }
    }
}