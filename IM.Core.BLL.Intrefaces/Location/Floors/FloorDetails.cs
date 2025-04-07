using System;

namespace InfraManager.BLL.Location.Floors;

public class FloorDetails : FloorData
{
    public int ID { get; init; }

    public Guid IMObjID { get; init; }
}