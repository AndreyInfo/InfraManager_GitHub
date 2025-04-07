using System;

namespace InfraManager.BLL.Asset.LifeCycleCommands.DeviceLocation;
public class DeviceLocationData
{
    public Guid? RoomID { get; init; }
    public Guid? RackID { get; init; }
    public Guid? NetworkDeviceID { get; init; }
}
