using System;

namespace InfraManager.BLL.Location.Rooms;

public class RoomData
{
    public string Name { get; init; }

    public string Note { get; init; }

    public int? TypeID { get; init; }

    public string Size { get; init; }

    public int? FloorID { get; init; }

    public int? ParentID { get; init; }

    public Guid? SubdivisionID { get; init; }

    public string ExternalID { get; init; }

    public byte[] RowVersion { get; init; }
}