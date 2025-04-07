using System;
using InfraManager.BLL.Location.Floors;

namespace InfraManager.BLL.Location.AdminLocation;

[Obsolete("Удалить, как только Admin будет использовать новый API")]
public class AdminFloorData : FloorData
{
    public int ID { get; init; }
}