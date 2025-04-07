using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

public class CalendarWorkScheduleShiftExclusionDetails
{
    public Guid ID { get; init; }
    public Guid CalendarWorkScheduleShiftID { get; init; }
    public Guid ExclusionID { get; init; }
    public string ExclusionName { get; init; }
    public DateTime TimeStart { get; init; }
    public DateTime TimeEnd { get; init; }
    public short TimeSpanInMinutes { get; init; }
}

