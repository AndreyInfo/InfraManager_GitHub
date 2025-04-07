using System;
using InfraManager.BLL.Location.Buildings;

namespace InfraManager.BLL.Location.AdminLocation;

[Obsolete("Удалить, как только Admin будет использовать новый API")]
public class AdminBuildingData : BuildingData
{
    public int ID { get; init; }
}