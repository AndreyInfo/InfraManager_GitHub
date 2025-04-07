using System;

namespace InfraManager.BLL.Location.Rooms;

public class RoomDetails : RoomData
{
    public int ID { get; init; }

    public Guid IMObjID { get; init; }
}