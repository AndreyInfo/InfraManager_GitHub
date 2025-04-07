using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Location.TimeZones;

public class TimeZoneListFilter : ClientPageFilter<TimeZone>
{
    public string Name { get; init; }
}