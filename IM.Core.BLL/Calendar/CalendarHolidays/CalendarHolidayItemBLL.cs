using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.CrudWeb;
using InfraManager.DAL;

namespace InfraManager.BLL.Calendar.CalendarHolidays;

internal class CalendarHolidayItemBLL : ICalendarHolidayItemBLL, ISelfRegisteredService<ICalendarHolidayItemBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<CalendarHolidayItem> _repository;
    private readonly IReadonlyRepository<CalendarHoliday> _repositoryCalendHoliday;
    private readonly IUnitOfWork _saveChangesCommand;

    public CalendarHolidayItemBLL(IMapper mapper,
                                   IRepository<CalendarHolidayItem> repository,
                                   IReadonlyRepository<CalendarHoliday> repositoryCalendHoliday,
                                   IUnitOfWork saveChangesCommand)
    {
        _mapper = mapper;
        _repository = repository;
        _repositoryCalendHoliday = repositoryCalendHoliday;
        _saveChangesCommand = saveChangesCommand;
    }

    public async Task<Guid> AddAsync(CalendarHolidayItemDetails model, CancellationToken cancellationToken)
    {
        await ThrowIfDoesntExistsCalendarHolidayAsync(model.CalendarHolidayID, cancellationToken);
        await ThrowIfExistsSameItemAsync(model, cancellationToken);

        var saveModel = _mapper.Map<CalendarHolidayItem>(model);
        _repository.Insert(saveModel);
        await _saveChangesCommand.SaveAsync(cancellationToken);

        return saveModel.ID;
    }

    public async Task<Guid> UpdateAsync(CalendarHolidayItemDetails model, CancellationToken cancellationToken)
    {
        await ThrowIfDoesntExistsCalendarHolidayAsync(model.CalendarHolidayID, cancellationToken);
        await ThrowIfExistsSameItemAsync(model, cancellationToken);

        var foundEntity = await _repository.FirstOrDefaultAsync(c => c.ID == model.ID, cancellationToken);
        if (foundEntity is null)
            throw new ObjectNotFoundException<Guid>(model.ID, "Not found CalendarHolidayItem");


        _mapper.Map(model, foundEntity);
        await _saveChangesCommand.SaveAsync(cancellationToken);

        return model.ID;
    }

    private async Task ThrowIfExistsSameItemAsync(CalendarHolidayItemDetails model, CancellationToken cancellationToken)
    {
        var isExists = await _repository.AnyAsync(c => c.Day == model.Day
                                                       && c.Month == model.Month
                                                       && c.CalendarHolidayID == model.CalendarHolidayID
                                                      , cancellationToken);

        if (isExists)
            throw new InvalidObjectException($"Выходной уже существует в данном календаре");
    }

    public async Task DeleteAsync(DeleteModel<Guid>[] models, CancellationToken cancellationToken)
    {
        var deleteModels = _mapper.Map<CalendarHolidayItem[]>(models);

        foreach (var item in deleteModels)
            _repository.Delete(item);

        await _saveChangesCommand.SaveAsync(cancellationToken);
    }

    public async Task<CalendarHolidayItemDetails[]> GetListAsync(Guid calendarHolidayID, string search = "", CancellationToken cancellationToken = default)
    {
        await ThrowIfDoesntExistsCalendarHolidayAsync(calendarHolidayID, cancellationToken);

        var query = _repository.Query().Where(c => c.CalendarHolidayID == calendarHolidayID);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(c => c.Day.ToString().Contains(search)
                                    || c.Month.ToString().Contains(search));

        var entites = await query.ExecuteAsync(cancellationToken);
        return _mapper.Map<CalendarHolidayItemDetails[]>(entites);
    }

    private async Task ThrowIfDoesntExistsCalendarHolidayAsync(Guid id, CancellationToken cancellationToken)
    {
        var isExistsCalendarHoliday = await _repositoryCalendHoliday.AnyAsync(c => c.ID == id, cancellationToken);
        if (!isExistsCalendarHoliday)
            throw new ObjectNotFoundException<Guid>(id, ObjectClass.CalendarHoliday);
    }

    public async Task<CalendarHolidayItemDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        var item = await _repository.FirstOrDefaultAsync(c => c.ID == id, cancellationToken);

        if (item is null)
            throw new ObjectNotFoundException("CalendarHolidayItem not found");

        return _mapper.Map<CalendarHolidayItemDetails>(item);
    }


}
