using AutoMapper;
using InfraManager.DAL;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;
using System.Transactions;
using Microsoft.Extensions.Logging;
using InfraManager.BLL.AccessManagement;

namespace InfraManager.BLL.Settings.Calendar;

/// <summary>
/// Может существовать всего один объект, небольше
/// </summary>
internal sealed class SupportSettingsCalendarBLL : ISupportSettingsCalendarBLL,
    ISelfRegisteredService<ISupportSettingsCalendarBLL>
{
    private readonly IRepository<CalendarWorkScheduleDefault> _calendarWorkScheduleDefaults;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<TimeZone> _timeZones;
    private readonly IEnumerable<ITimeZoneObjects> _timeZoneObjects;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<SupportSettingsCalendarBLL> _logger;
    private readonly IUserAccessBLL _userAccessBLL;

    public SupportSettingsCalendarBLL(IRepository<CalendarWorkScheduleDefault> calendarWorkScheduleDefaults
        , IUnitOfWork unitOfWork
        , IMapper mapper
        , IReadonlyRepository<TimeZone> timeZones
        , IEnumerable<ITimeZoneObjects> array
        , ICurrentUser currentUser
        , ILogger<SupportSettingsCalendarBLL> logger
        , IUserAccessBLL userAccessBLL)
    {
        _calendarWorkScheduleDefaults = calendarWorkScheduleDefaults;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _timeZones = timeZones;
        _timeZoneObjects = array;
        _currentUser = currentUser;
        _logger = logger;
        _userAccessBLL = userAccessBLL;
    }

    public async Task<CalendarSettingsDetails> GetAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request get {nameof(CalendarWorkScheduleDefault)}");
        await _userAccessBLL.ThrowIfNoAdminAsync(_currentUser.UserId, _logger, cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} start get {nameof(CalendarWorkScheduleDefault)}");

        var entity = await _calendarWorkScheduleDefaults.FirstOrDefaultAsync(cancellationToken)
            ?? throw new ObjectNotFoundException("Not found SupportSettings Calendar");

        return _mapper.Map<CalendarSettingsDetails>(entity);
    }

    public async Task<CalendarSettingsDetails> UpdateAsync(CalendarSettingsDetails model, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request update {nameof(CalendarWorkScheduleDefault)}");
        await _userAccessBLL.ThrowIfNoAdminAsync(_currentUser.UserId, _logger, cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} start update {nameof(CalendarWorkScheduleDefault)}");

        var entity = await _calendarWorkScheduleDefaults.FirstOrDefaultAsync(cancellationToken)
            ?? throw new ObjectNotFoundException("Not found SupportSettings Calendar");

        _mapper.Map(model, entity);

        await _unitOfWork.SaveAsync(cancellationToken);
        
        _logger.LogTrace($"UserID = {_currentUser.UserId} finish update {nameof(CalendarWorkScheduleDefault)}");
        return await GetAsync(cancellationToken);
    }

    public async Task UpdateObjectTimeZoneAsync(string timeZoneID, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request update objects with timezone");
        await _userAccessBLL.ThrowIfNoAdminAsync(_currentUser.UserId, _logger, cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} start update objects with timezone");

        var isExists = await _timeZones.AnyAsync(c => c.ID.Equals(timeZoneID), cancellationToken);
        if (!isExists)
            throw new ObjectNotFoundException<string>(timeZoneID, "Not found TimeZone");


        using var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, 
                                                               TransactionScopeOption.Required);
        foreach (var item in _timeZoneObjects)
        {
            await item.UpdateTimeZoneObjectsAsync(timeZoneID, cancellationToken);
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        transaction.Complete();

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish update objects with timezone");
    }
}
