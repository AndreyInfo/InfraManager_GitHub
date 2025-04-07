using System;
using InfraManager.BLL.Location.Workplaces;

namespace InfraManager.BLL.Location.AdminLocation;

[Obsolete("Удалить, как только Admin будет использовать новый API")]
public class AdminWorkplaceData : WorkplaceData
{
    public int ID { get; init; }
}