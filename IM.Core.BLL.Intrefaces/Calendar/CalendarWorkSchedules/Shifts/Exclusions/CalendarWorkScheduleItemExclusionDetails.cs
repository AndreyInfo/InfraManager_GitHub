using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

public class CalendarWorkScheduleItemExclusionDetails
{
    public Guid ID { get; init; }

    public Guid CalendarWorkScheduleID { get; init; }

    public short DayOfYear { get; init; }

    public string ExclusionName { get; init; }

    public Guid ExclusionID { get; init; }

    public DateTime TimeStart { get; init; }

    public short TimeSpanInMinutes { get; init; }
}

