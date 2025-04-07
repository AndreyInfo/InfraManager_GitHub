using System;

namespace InfraManager.BLL.Location.Buildings;

public class BuildingDetails : BuildingData
{
    public int ID { get; init; }

    public Guid IMObjID { get; init; }
}