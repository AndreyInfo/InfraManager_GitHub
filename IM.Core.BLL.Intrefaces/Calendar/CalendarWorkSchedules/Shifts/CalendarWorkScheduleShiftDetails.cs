using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts;

public class CalendarWorkScheduleShiftDetails
{
    public Guid ID { get; set; }

    public Guid CalendarWorkScheduleID { get; set; }

    public byte Number { get; set; }

    public DateTime TimeStart { get; set; }

    public short TimeSpanInMinutes { get; set; }

    public int TotalTimeSpanInMinutes { get; set; }
}

