using System;

namespace InfraManager.BLL.Location.Workplaces;

public class WorkplaceListFilter
{
    public string Name { get; init; }

    public Guid? IMObjID { get; init; }

    public int? RoomID { get; init; }

    public Guid? RoomIMObjID { get; init; }
}