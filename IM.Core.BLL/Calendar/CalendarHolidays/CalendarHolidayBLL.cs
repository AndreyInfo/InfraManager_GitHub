using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using Inframanager.BLL;
using Microsoft.Extensions.Logging;
using Inframanager.BLL.AccessManagement;
using Inframanager;

namespace InfraManager.BLL.Calendar.CalendarHolidays;

internal sealed class CalendarHolidayBLL : ICalendarHolidayBLL
    , ISelfRegisteredService<ICalendarHolidayBLL>
{
    private readonly IRepository<CalendarHoliday> _repositoryCalendarHolidays;
    private readonly IRepository<CalendarHolidayItem> _repositoryCalendarHolidayItems;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<CalendarWorkScheduleDefault> _calendarWorkScheduleDefaults;
    private readonly IValidatePermissions<CalendarHoliday> _validatePermissions;
    private readonly ILogger<CalendarHolidayBLL> _logger;
    private readonly ICurrentUser _currentUser;
    public CalendarHolidayBLL(IRepository<CalendarHoliday> repositoryCalendarHolidays,
                              IRepository<CalendarHolidayItem> repositoryCalendarHolidayItems,
                              IUnitOfWork unitOfWork,
                              IMapper mapper,
                              IReadonlyRepository<CalendarWorkScheduleDefault> calendarWorkScheduleDefaults,
                              IValidatePermissions<CalendarHoliday> validatePermissions,
                              ILogger<CalendarHolidayBLL> logger,
                              ICurrentUser currentUser
                              )
    {
        _repositoryCalendarHolidayItems = repositoryCalendarHolidayItems;
        _repositoryCalendarHolidays = repositoryCalendarHolidays;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _calendarWorkScheduleDefaults = calendarWorkScheduleDefaults;
        _validatePermissions = validatePermissions;
        _logger = logger;
        _currentUser = currentUser;
    }


    public async Task<Guid> AddAsync(CalendarHolidayInsertDetails model, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);

        var saveModel = _mapper.Map<CalendarHoliday>(model);
        saveModel.CalendarHolidayItems.ForEach(c => c.CalendarHolidayID = saveModel.ID);
        _repositoryCalendarHolidays.Insert(saveModel);

        await SaveCalendarHolidayItemsAsync(saveModel.CalendarHolidayItems, saveModel.ID, cancellationToken);

        await _unitOfWork.SaveAsync(cancellationToken);
        return saveModel.ID;
    }

    public async Task<Guid> UpdateAsync(CalendarHolidayDetails model, Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

        var existsEntity = await _repositoryCalendarHolidays.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.CalendarHoliday);

        existsEntity = _mapper.Map(model, existsEntity);
        await SaveCalendarHolidayItemsAsync(existsEntity.CalendarHolidayItems, existsEntity.ID, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return existsEntity.ID;
    }

    private async Task SaveCalendarHolidayItemsAsync(IEnumerable<CalendarHolidayItem> items, Guid calendarHolidayID, CancellationToken cancellationToken)
    {
        var existsItems = await _repositoryCalendarHolidayItems.ToArrayAsync(c => c.CalendarHolidayID == calendarHolidayID, cancellationToken);

        existsItems.ForEach(c => _repositoryCalendarHolidayItems.Delete(c));

        items.ForEach(c => _repositoryCalendarHolidayItems.Insert(c));
    }

    public async Task<CalendarHolidayDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var entity = await _repositoryCalendarHolidays.WithMany(c => c.CalendarHolidayItems)
                                                      .FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                                                      ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.CalendarHoliday);

        return _mapper.Map<CalendarHolidayDetails>(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var entity = await _repositoryCalendarHolidays.WithMany(c => c.CalendarHolidayItems)
                                                      .FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                                                      ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.CalendarHoliday);


        _repositoryCalendarHolidays.Delete(entity);

        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} deleted {nameof(CalendarHoliday)}");
    }
}
