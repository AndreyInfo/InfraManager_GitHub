using System;

namespace InfraManager.BLL.Asset.Adapters;

public class AdapterData
{
    public string Name { get; init; }
    public string Note { get; init; }
    public string SerialNumber { get; init; }
    public string Code { get; init; }
    public bool IsWorking { get; init; }
    public Guid? LifeCycleStateID { get; init; }
    public Guid? OwnerID { get; init; }
    public Guid? UtilizerID { get; init; }
    public int? RoomID { get; init; }
    public int? UserID { get; init; }
    public bool OnStore { get; init; }

    //TODO: Пока заглушки. Будет реализовано в рамках подклассов. Согласовано.
    public string ChipType { get; init; }
    public string ConverterType { get; init; }
    public string MemoryCapacity { get; init; }
    public string Modes { get; init; }

    public Guid? AdapterTypeID { get; init; }
    public int? TerminalDeviceID { get; init; }
    public int? NetworkDeviceID { get; init; }
    public string Identifier { get; init; }
    public string ExternalID { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public string InventoryNumber { get; set; }
}