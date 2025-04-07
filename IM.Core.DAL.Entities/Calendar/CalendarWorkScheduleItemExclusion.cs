using InfraManager.DAL.Calendar;
using System;

namespace InfraManager.DAL;

public class CalendarWorkScheduleItemExclusion
{
    public Guid ID { get; init; }
    public Guid CalendarWorkScheduleID { get; init; }
    public short DayOfYear { get; init; }
    public Guid ExclusionID { get; init; }
    public DateTime TimeStart { get; init; }
    public short TimeSpanInMinutes { get; init; }
    public virtual Exclusion Exclusion { get; }
}
