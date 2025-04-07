using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts;

public class CalendarWorkScheduleShiftCreateData
{
    public Guid CalendarWorkScheduleID { get; init; }

    public byte Number { get; init; }

    public DateTime TimeStart { get; init; }

    public short TimeSpanInMinutes { get; init; }

}

