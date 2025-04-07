using System;

namespace InfraManager.BLL.Location.Rooms;

public class RoomListFilter
{
    public string Name { get; init; }

    public Guid? IMObjID { get; init; }

    public int? FloorID { get; init; }
}