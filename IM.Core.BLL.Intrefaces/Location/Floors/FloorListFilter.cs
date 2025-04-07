using System;

namespace InfraManager.BLL.Location.Floors;

public class FloorListFilter
{
    public string Name { get; init; }

    public Guid? IMObjID { get; init; }

    public int? BuildingID { get; init; }
}