using System;

namespace InfraManager.BLL.Asset.Adapters;

public class AdapterDetails
{
    public Guid IMObjID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public string SerialNumber { get; init; }
    public string Code { get; init; }
    public string VendorName { get; init; }
    public int VendorID { get; init; }
    public bool IsWorking { get; init; }
    public Guid? LifeCycleStateID { get; init; }
    public string Classification { get; init; }
    public DateTime? DateReceived { get; init; }
    public string Owner { get; set; }
    public Guid? OwnerID { get; init; }
    public string Utilizer { get; set; }
    public Guid? UtilizerID { get; init; }
    public bool OnStore { get; init; }
    public string Location { get; set; }
    public string OrganizationName { get; init; }
    public Guid? OrganizationID { get; init; }
    public string BuildingName { get; init; }
    public int? BuildingID { get; init; }
    public string FloorName { get; init; }
    public int? FloorID { get; init; }
    public string RoomName { get; init; }
    public int? RoomID { get; init; }
    public string WorkplaceName { get; set; }
    public int? WorkplaceID { get; set; }
    public string RackName { get; set; }
    public int? RackID { get; set; }
    public string InventoryNumber { get; set; }

    //TODO: Пока заглушки. Будет реализовано в рамках подклассов. Согласовано.
    public string ChipType { get; init; }
    public string ConverterType { get; init; }
    public string MemoryCapacity { get; init; }
    public string Modes { get; init; }

    public Guid? AdapterTypeID { get; init; }
    public int TerminalDeviceID { get; init; }
    public int NetworkDeviceID { get; init; }
    public Guid? ComplementaryID { get; init; }
    public int? ComplementaryIntID { get; init; }
    public string Identifier { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public string ExternalID { get; init; }
    public int? TemplateID { get; set; }
}