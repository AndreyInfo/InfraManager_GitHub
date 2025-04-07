using InfraManager.DAL.Calendar;
using System;

namespace InfraManager.DAL.CalendarWorkSchedules;

public class CalendarWorkScheduleShiftExclusion
{
    public CalendarWorkScheduleShiftExclusion()
    {

    }

    public CalendarWorkScheduleShiftExclusion(Guid calendarWorkScheduleShiftID
                                              , Guid exclusionID
                                              , DateTime timeStart
                                              , short timeSpanInMinutes)
    {
        ID = Guid.NewGuid();
        CalendarWorkScheduleShiftID = calendarWorkScheduleShiftID;
        ExclusionID = exclusionID;
        TimeStart = timeStart;
        TimeSpanInMinutes = timeSpanInMinutes;
    }

    public Guid ID { get; init; }
    public Guid CalendarWorkScheduleShiftID { get; init; }
    public Guid ExclusionID { get; init; }

    public DateTime TimeStart { get; init; }

    public short TimeSpanInMinutes { get; init; }

    public virtual Exclusion Exclusion { get; init; }

    public virtual CalendarWorkScheduleShift CalendarWorkScheduleShift { get; init; }
}
