using System;
using InfraManager.BLL.Location.Rooms;

namespace InfraManager.BLL.Location.Workplaces;

public class WorkplaceData
{
    public string Name { get; init; }

    public string Note { get; init; }

    public int? RoomID { get; init; }

    public int? ParentId { get; init; }

    public Guid? SubdivisionID { get; init; }

    public byte[] RowVersion { get; init; }

    public string ExternalID { get; init; }

    public RoomDetails Room { get; init; }

    public WorkplaceBuildingInfo BuildingInfo { get; init; }
}