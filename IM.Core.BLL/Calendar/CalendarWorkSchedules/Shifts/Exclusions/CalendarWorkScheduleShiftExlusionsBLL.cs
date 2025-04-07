using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

internal sealed class CalendarWorkScheduleShiftExlusionsBLL : ICalendarWorkScheduleShiftExlusionsBLL
    , ISelfRegisteredService<ICalendarWorkScheduleShiftExlusionsBLL>
{
    private readonly IRepository<CalendarWorkScheduleShiftExclusion> _repositoryCalendarWorkScheduleShiftExclusion;
    private readonly IGuidePaggingFacade<CalendarWorkScheduleShiftExclusion, CalendarWorkScheduleShiftExclusionsColumns> _guidePaggingFacade;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CalendarWorkScheduleShiftExlusionsBLL(IMapper mapper,
        IRepository<CalendarWorkScheduleShiftExclusion> repositoryCalendarWorkScheduleShiftExclusion,
        IGuidePaggingFacade<CalendarWorkScheduleShiftExclusion, CalendarWorkScheduleShiftExclusionsColumns> guidePaggingFacade,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _repositoryCalendarWorkScheduleShiftExclusion = repositoryCalendarWorkScheduleShiftExclusion;
        _guidePaggingFacade = guidePaggingFacade;
        _unitOfWork = unitOfWork;
    }

    public async Task<CalendarWorkScheduleShiftExclusionDetails[]> GetExclutionsAsync(FilterShiftsExclusion filter, CancellationToken cancellationToken = default)
    {
        var query = _repositoryCalendarWorkScheduleShiftExclusion.With(c => c.Exclusion)
                                                                 .Query()
                                                                 .Where(c => c.CalendarWorkScheduleShiftID == filter.CalendarWorkSheduleShiftID);

        if (filter.Type.HasValue)
            query = query.Where(c => c.Exclusion.Type == filter.Type);

        var entites = await _guidePaggingFacade.GetPaggingAsync(filter, query, null, cancellationToken);
        return _mapper.Map<CalendarWorkScheduleShiftExclusionDetails[]>(entites);
    }

    public async Task UpdateExclutionAsync(CalendarWorkScheduleShiftExclusionDetails exclusionDetails, CancellationToken cancellationToken = default)
    {
        var entity = await _repositoryCalendarWorkScheduleShiftExclusion.FirstOrDefaultAsync(c => c.ID == exclusionDetails.ID, cancellationToken);

        _mapper.Map(exclusionDetails, entity);

        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task<Guid> CreateExclutionAsync(CreateCalendarWorkScheduleShiftExclusionDetails exclusionDetails, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<CalendarWorkScheduleShiftExclusion>(exclusionDetails);
        _repositoryCalendarWorkScheduleShiftExclusion.Insert(entity);
        await _unitOfWork.SaveAsync(cancellationToken);
        return entity.ID;
    }

    public async Task DeleteExclusionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repositoryCalendarWorkScheduleShiftExclusion.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                        ?? throw new ObjectNotFoundException<Guid>(id, $"Not found {nameof(CalendarWorkScheduleShiftExclusion)}");

        _repositoryCalendarWorkScheduleShiftExclusion.Delete(entity);

        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task<int> GetExclutionsTotalTimeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repositoryCalendarWorkScheduleShiftExclusion.ToArrayAsync (c => c.CalendarWorkScheduleShiftID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, $"Not found {nameof(CalendarWorkScheduleShift)}");

        return entity.Sum(c => c.TimeSpanInMinutes);
    }
}
