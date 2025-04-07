using System;

namespace InfraManager.BLL.Asset.Peripherals;

public class PeripheralData
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
    public bool? OnStore { get; init; }

    //TODO: Пока заглушки. Будет реализовано в рамках подклассов. Согласовано.
    public string PortType { get; init; }
    public string MaxLoad { get; init; }
    public string MinLoad { get; init; }
    public bool? ColorPrint { get; init; }
    public bool? FotoPrint { get; init; }

    public Guid PeripheralTypeID { get; init; }
    public int TerminalDeviceID { get; init; }
    public int NetworkDeviceID { get; init; }
    public string ExternalID { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
}