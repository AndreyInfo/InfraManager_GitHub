using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.CalendarService;
using InfraManager.BLL.OrganizationStructure.Groups;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.Settings;
using InfraManager.BLL.SlaUtils;
using InfraManager.BLL.Users;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.Users;

namespace InfraManager.BLL.ServiceDesk;

internal class WorkloadBLL :
    IEmployeeWorkloadBLL,
    IGroupWorkloadBLL,
    ISelfRegisteredService<IEmployeeWorkloadBLL>,
    ISelfRegisteredService<IGroupWorkloadBLL>
{
    // NOTE: Возможно стоит унести в настройки. Время, в течение которого пользователь считается активным. По-умолчанию 15 минут.
    private const long UserActivityTicks = 9000000000;

    private readonly ICurrentUser _currentUser;
    private readonly IEmployeeWorkloadQueryBuilder _employeeWorkloadQueryBuilder;
    private readonly IGroupWorkloadQueryBuilder _groupWorkloadQueryBuilder;
    private readonly ICalendarServiceBLL _calendarService;
    private readonly IPagingQueryCreator _paging;
    private readonly IMapper _mapper;
    private readonly IConvertSettingValue<long> _longConverter;
    private readonly ISettingsBLL _settings;
    private readonly IReadonlyRepository<CalendarWorkScheduleDefault> _defaultCalendarRepository;

    public WorkloadBLL(
        ICurrentUser currentUser,
        IEmployeeWorkloadQueryBuilder employeeWorkloadQueryBuilder,
        IGroupWorkloadQueryBuilder groupWorkloadQueryBuilder,
        ICalendarServiceBLL calendarService,
        IPagingQueryCreator paging,
        IMapper mapper,
        IConvertSettingValue<long> longConverter,
        ISettingsBLL settings,
        IReadonlyRepository<CalendarWorkScheduleDefault> defaultCalendarRepository)
    {
        _currentUser = currentUser;
        _employeeWorkloadQueryBuilder = employeeWorkloadQueryBuilder;
        _groupWorkloadQueryBuilder = groupWorkloadQueryBuilder;
        _calendarService = calendarService;
        _paging = paging;
        _mapper = mapper;
        _longConverter = longConverter;
        _settings = settings;
        _defaultCalendarRepository = defaultCalendarRepository;
    }

    public async Task<EmployeeWorkloadListItem[]> GetEmployeeWorkloadReportAsync(WorkloadListData data, ClientPageFilter pageBy, CancellationToken cancellationToken = default)
    {
        var utcStartDate = DateTime.UtcNow;

        var query = _employeeWorkloadQueryBuilder.BuildQuery(utcStartDate, utcStartDate.AddTicks(-UserActivityTicks));
        var pagingQuery = _paging.Create(query.OrderBy(x => x.FullName));
        var queryItems = await pagingQuery.PageAsync(pageBy.Skip, pageBy.Take, cancellationToken);
        var items = _mapper.Map<EmployeeWorkloadListItem[]>(queryItems);

        var datePromised = await GetDatePromisedAsync(utcStartDate, data, cancellationToken);
        if (datePromised <= utcStartDate)
        {
            return items;
        }

        var totalWorkHours = await GetTotalWorkHoursAsync(utcStartDate, datePromised, cancellationToken);
        foreach (var item in items)
        {
            var availableWorkHours = await GetUserWorkTimeAsync(utcStartDate, datePromised, item.ID, cancellationToken);
            var (availableHours, availablePercent) = CalculateAvailableTimes(availableWorkHours, totalWorkHours);

            item.AvailableHours = availableHours;
            item.AvailablePercent = availablePercent;
        }

        return items;
    }

    public async Task<GroupWorkloadListItem[]> GetGroupWorkloadReportAsync(WorkloadListData data, ClientPageFilter pageBy, CancellationToken cancellationToken = default)
    {
        var utcStartDate = DateTime.UtcNow;

        var type = data.ClassID switch
        {
            ObjectClass.Call => GroupType.Call,
            ObjectClass.WorkOrder => GroupType.WorkOrder,
            ObjectClass.Problem => GroupType.Problem,
            ObjectClass.ChangeRequest => GroupType.ChangeRequest,
            ObjectClass.MassIncident => GroupType.MassiveIncident,
            _ => GroupType.None,
        };

        var query = _groupWorkloadQueryBuilder.BuildQuery(utcStartDate, _currentUser.UserId, type);
        var pagingQuery = _paging.Create(query.OrderBy(x => x.FullName));
        var queryItems = await pagingQuery.PageAsync(pageBy.Skip, pageBy.Take, cancellationToken);
        var items = _mapper.Map<GroupWorkloadListItem[]>(queryItems);

        var datePromised = await GetDatePromisedAsync(utcStartDate, data, cancellationToken);
        if (datePromised <= utcStartDate)
        {
            return items;
        }

        var totalWorkHours = await GetTotalWorkHoursAsync(utcStartDate, datePromised, cancellationToken);
        foreach (var item in items)
        {
            var availableWorkHours = await GetGroupWorkTimeAsync(utcStartDate, datePromised, item.ID, cancellationToken);
            var (availableHours, availablePercent) = CalculateAvailableTimes(availableWorkHours, totalWorkHours);

            item.AvailableHours = availableHours;
            item.AvailablePercent = availablePercent;
        }

        return items;
    }

    private static (decimal?, decimal?) CalculateAvailableTimes(decimal availableWorkHours, decimal totalWorkHours)
    {
        const int hourDecimals = 2; // NOTE: Часы округляются до двух знаков после запятой (переехало из legacy)
        
        var availableHours = availableWorkHours == 0
            ? (decimal?) null
            : Math.Round(availableWorkHours, hourDecimals);

        var availableTimeRatio = availableWorkHours / totalWorkHours;
        var availablePercent = totalWorkHours == 0 && availableHours == 0
            ? (decimal?) null
            : Math.Round(Math.Min(1, availableTimeRatio) * 100m, hourDecimals);

        return (availableHours, availablePercent);
    }

    private async Task<decimal> GetTotalWorkHoursAsync(DateTime utcStartDate, DateTime utcFinishDate, CancellationToken cancellationToken)
    {
        return (decimal) (await _calendarService.GetWorkTimeByCalendarAsync(
                new WorkTimeByCalendarData
                {
                    utcStartDate = utcStartDate,
                    utcFinishDate = utcFinishDate,
                }, cancellationToken))
            .TotalHours;
    }

    private async Task<decimal> GetUserWorkTimeAsync(DateTime utcStartDate, DateTime utcFinishDate, Guid userID, CancellationToken cancellationToken)
    {
        return (decimal) (await _calendarService.GetWorkTimeByUserAsync(
                new WorkTimeByUserData
                {
                    utcStartDate = utcStartDate,
                    utcFinishDate = utcFinishDate,
                    UserID = userID,
                }, cancellationToken))
            .TotalHours;
    }

    private async Task<decimal> GetGroupWorkTimeAsync(DateTime utcStartDate, DateTime utcFinishDate, Guid groupID, CancellationToken cancellationToken)
    {
        return (decimal) (await _calendarService.GetWorkTimeByGroupAsync(
                new WorkTimeByGroupData
                {
                    utcStartDate = utcStartDate,
                    utcFinishDate = utcFinishDate,
                    GroupID = groupID,
                }, cancellationToken))
            .TotalHours;
    }

    private async Task<DateTime> GetDatePromisedAsync(DateTime utcStartDate, WorkloadListData data, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(data.UtcDatePromisedMilliseconds))
        {
            return data.UtcDatePromisedMilliseconds.ConvertFromMillisecondsAfterMinimumDate();
        }

        var defaultCalendar = await _defaultCalendarRepository
            .With(x => x.CalendarHoliday)
            .With(x => x.CalendarWeekend)
            .With(x => x.TimeZone)
            .ThenWithMany(x => x.TimeZoneAdjustmentRules)
            .FirstOrDefaultAsync(cancellationToken);

        var promiseTicks = data.ClassID switch
        {
            ObjectClass.Call => _longConverter.Convert(await _settings.GetValueAsync(SystemSettings.CallPromiseDateDelta, cancellationToken)),
            ObjectClass.WorkOrder => _longConverter.Convert(await _settings.GetValueAsync(SystemSettings.WorkOrderFinishDateDelta, cancellationToken)),
            ObjectClass.Problem => _longConverter.Convert(await _settings.GetValueAsync(SystemSettings.ProblemPromiseDateDelta, cancellationToken)),
            _ => throw new ArgumentOutOfRangeException(nameof(data.ClassID)),
        };

        return SlaHelper.GetUtcFinishDateByCalendar(utcStartDate, new TimeSpan(promiseTicks), null, null, defaultCalendar);
    }
}