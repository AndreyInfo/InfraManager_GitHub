using AutoMapper;
using Inframanager;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;

internal sealed class CalendarWorkScheduleItemBLL : ICalendarWorkScheduleItemBLL
    , ISelfRegisteredService<ICalendarWorkScheduleItemBLL>
{
    private readonly IGuidePaggingFacade<CalendarWorkScheduleItem, DaysForTable> _guidePaggingFacade;
    private readonly ILogger<CalendarWorkScheduleItemBLL> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;
    private readonly IRepository<CalendarWorkScheduleItem> _calendarWorkScheduleItems;

    public CalendarWorkScheduleItemBLL(IGuidePaggingFacade<CalendarWorkScheduleItem, DaysForTable> guidePaggingFacade
        , ILogger<CalendarWorkScheduleItemBLL> logger
        , ICurrentUser currentUser
        , IMapper mapper
        , IRepository<CalendarWorkScheduleItem> calendarWorkScheduleItems)
    {
        _guidePaggingFacade = guidePaggingFacade;
        _logger = logger;
        _currentUser = currentUser;
        _mapper = mapper;
        _calendarWorkScheduleItems = calendarWorkScheduleItems;
    }

    public async Task<CalendarWorkScheduleItemDetails[]> GetByFilterAsync(BaseFilter filter, Guid calendarworkScheduleID, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} start {ObjectAction.ViewDetailsArray} {nameof(CalendarWorkScheduleItem)} without recalculate");

        var query = _calendarWorkScheduleItems.Query().Where(c => c.CalendarWorkScheduleID == calendarworkScheduleID);

        var items = await _guidePaggingFacade.GetPaggingAsync(filter
            , query
            , c => c.DayOfYear.ToString().Contains(filter.SearchString)
            , cancellationToken);

        return _mapper.Map<CalendarWorkScheduleItemDetails[]>(items);
    }
}
