using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Location.TimeZones;

internal class TimeZoneBLL : ITimeZoneBLL, ISelfRegisteredService<ITimeZoneBLL>
{
    private readonly IGetEntityArrayBLL<string, TimeZone, TimeZoneDetails, TimeZoneListFilter> _getTimeZonesArrayBll;
    private readonly IGetEntityBLL<string, TimeZone, TimeZoneDetails> _getTimeZoneEntityBll;

    public TimeZoneBLL(
        IGetEntityArrayBLL<string, TimeZone, TimeZoneDetails, TimeZoneListFilter> getTimeZonesArrayBll,
        IGetEntityBLL<string, TimeZone, TimeZoneDetails> getTimeZoneEntityBll)
    {
        _getTimeZonesArrayBll = getTimeZonesArrayBll;
        _getTimeZoneEntityBll = getTimeZoneEntityBll;
    }

    public async Task<IEnumerable<TimeZoneDetails>> GetDataForTableAsync(TimeZoneListFilter filter, CancellationToken cancellationToken = default)
        => await _getTimeZonesArrayBll.PageAsync(filter, filter, cancellationToken);

    public async Task<TimeZoneDetails> GetAsync(string id, CancellationToken cancellationToken = default)
     => await _getTimeZoneEntityBll.DetailsAsync(id, cancellationToken);
}
