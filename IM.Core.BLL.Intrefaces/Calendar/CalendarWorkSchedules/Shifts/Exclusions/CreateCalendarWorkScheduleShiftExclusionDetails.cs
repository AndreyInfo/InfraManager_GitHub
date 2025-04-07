using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

public class CreateCalendarWorkScheduleShiftExclusionDetails
{
    public Guid CalendarWorkScheduleShiftID { get; init; }

    public Guid ExclusionID { get; init; }

    public DateTime TimeStart { get; init; }

    public short TimeSpanInMinutes { get; init; }
}

