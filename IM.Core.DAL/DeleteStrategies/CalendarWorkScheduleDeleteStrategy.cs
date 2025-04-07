using InfraManager.DAL.CalendarWorkSchedules;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.DeleteStrategies;

internal sealed class CalendarWorkScheduleDeleteStrategy : IDeleteStrategy<CalendarWorkSchedules.CalendarWorkSchedule>
    , ISelfRegisteredService<IDeleteStrategy<CalendarWorkSchedules.CalendarWorkSchedule>>
{
    private readonly IRepository<CalendarWorkScheduleItem> _calendarWorkScheduleItems;
    private readonly IDeleteStrategy<CalendarWorkScheduleShift> _calendarWorkScheduleShifts;
    private readonly DbSet<CalendarWorkSchedules.CalendarWorkSchedule> _calendarWorkSchedules;

    public CalendarWorkScheduleDeleteStrategy(IRepository<CalendarWorkScheduleItem> calendarWorkScheduleItems
                                              , IDeleteStrategy<CalendarWorkScheduleShift> calendarWorkScheduleShifts
                                              , DbSet<CalendarWorkSchedules.CalendarWorkSchedule> calendarWorkSchedules)
    {
        _calendarWorkScheduleItems = calendarWorkScheduleItems;
        _calendarWorkScheduleShifts = calendarWorkScheduleShifts;
        _calendarWorkSchedules = calendarWorkSchedules;
    }


    public void Delete(CalendarWorkSchedules.CalendarWorkSchedule entity)
    {
        entity.WorkScheduleItems.ForEach(c => _calendarWorkScheduleItems.Delete(c));
        entity.WorkScheduleShifts.ForEach(c => _calendarWorkScheduleShifts.Delete(c));
        _calendarWorkSchedules.Remove(entity);
    }
}
