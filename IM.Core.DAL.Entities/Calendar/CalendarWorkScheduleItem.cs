using InfraManager.DAL.CalendarWorkSchedules;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL;

public class CalendarWorkScheduleItem
{
    public Guid CalendarWorkScheduleID { get; init; }
    public short DayOfYear { get; set; }
    public DateTime TimeStart { get; set; }
    public short TimeSpanInMinutes { get; set; }
    public byte? ShiftNumber { get; set; }
    public CalendarDayType DayType { get; set; }

    public virtual CalendarWorkSchedule CalendarWorkSchedule { get; init; }

    public virtual ICollection<CalendarWorkScheduleItemExclusion> WorkScheduleItemExclusions { get; init; }
}
