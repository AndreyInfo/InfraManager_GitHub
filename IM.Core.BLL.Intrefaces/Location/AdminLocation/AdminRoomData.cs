using System;
using InfraManager.BLL.Location.Rooms;

namespace InfraManager.BLL.Location.AdminLocation;

[Obsolete("Удалить, как только Admin будет использовать новый API")]
public class AdminRoomData : RoomData
{
    public int ID { get; init; }
}