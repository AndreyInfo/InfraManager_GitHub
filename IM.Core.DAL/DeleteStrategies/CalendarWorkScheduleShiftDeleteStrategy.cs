using InfraManager.DAL.CalendarWorkSchedules;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.DeleteStrategies;

internal sealed class CalendarWorkScheduleShiftDeleteStrategy : IDeleteStrategy<CalendarWorkScheduleShift>
    , ISelfRegisteredService<IDeleteStrategy<CalendarWorkScheduleShift>>
{
    private readonly IRepository<CalendarWorkScheduleShiftExclusion> _calendarWorkScheduleShiftExclusions;
    private readonly DbSet<CalendarWorkScheduleShift> _calendarWorkScheduleShifts;

    public CalendarWorkScheduleShiftDeleteStrategy(IRepository<CalendarWorkScheduleShiftExclusion> calendarWorkScheduleShiftExclusions
        , DbSet<CalendarWorkScheduleShift> calendarWorkScheduleShifts)
    {
        _calendarWorkScheduleShiftExclusions = calendarWorkScheduleShiftExclusions;
        _calendarWorkScheduleShifts = calendarWorkScheduleShifts;
    }
    public void Delete(CalendarWorkScheduleShift entity)
    {
        entity.WorkScheduleShiftExclusions.ForEach(c => _calendarWorkScheduleShiftExclusions.Delete(c));
        _calendarWorkScheduleShifts.Remove(entity);
    }
}
