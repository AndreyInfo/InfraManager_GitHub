using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Calendar.CalendarWeekends;

internal sealed class CalendarWeekendBLL : ICalendarWeekenedBLL
    , ISelfRegisteredService<ICalendarWeekenedBLL>
{
    private readonly IRepository<CalendarWeekend> _repositoryCalendarWeekends;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadonlyRepository<CalendarWorkScheduleDefault> _calendarWorkScheduleDefaults;
    private readonly IValidatePermissions<CalendarWeekend> _validatePermissions;
    private readonly ILogger<CalendarWeekendBLL> _logger;
    private readonly ICurrentUser _currentUser;

    public CalendarWeekendBLL(
        IRepository<CalendarWeekend> repositoryCalendarWeekends,
        IUnitOfWork unitOfWork,
        IReadonlyRepository<CalendarWorkScheduleDefault> calendarWorkScheduleDefaults,
        IValidatePermissions<CalendarWeekend> validatePermissions,
        ILogger<CalendarWeekendBLL> logger,
        ICurrentUser currentUser)
    {
        _repositoryCalendarWeekends = repositoryCalendarWeekends;
        _unitOfWork = unitOfWork;
        _calendarWorkScheduleDefaults = calendarWorkScheduleDefaults;
        _validatePermissions = validatePermissions;
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

        var entity = await _repositoryCalendarWeekends.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.CalendarWeekend);

        _repositoryCalendarWeekends.Delete(entity);

        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} deleted {nameof(CalendarWeekend)}");
    }
}
