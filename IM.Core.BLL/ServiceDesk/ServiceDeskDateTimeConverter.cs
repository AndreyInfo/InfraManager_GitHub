using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;

namespace InfraManager.BLL.ServiceDesk;

internal class ServiceDeskDateTimeConverter : IServiceDeskDateTimeConverter, ISelfRegisteredService<IServiceDeskDateTimeConverter>
{
    private const int DefaultUtcOffsetInMinutes = 0;

    private readonly IReadonlyRepository<CalendarWorkScheduleDefault> _calendarWorkScheduleDefaults;

    public ServiceDeskDateTimeConverter(IReadonlyRepository<CalendarWorkScheduleDefault> calendarWorkScheduleDefaults)
    {
        _calendarWorkScheduleDefaults = calendarWorkScheduleDefaults;
    }

    public async Task<DateTimeOffset> ConvertAsync(DateTime dateTime, CancellationToken cancellationToken = default)
    {
        var baseUtcOffsetInMinutes = (await _calendarWorkScheduleDefaults
                .With(x => x.TimeZone)
                .FirstOrDefaultAsync(cancellationToken)
            )?.TimeZone?.BaseUtcOffsetInMinutes ?? DefaultUtcOffsetInMinutes;

        return new DateTimeOffset(dateTime.Ticks, TimeSpan.Zero)
            .ToOffset(TimeSpan.FromMinutes(baseUtcOffsetInMinutes));
    }
}