using InfraManager.DAL.CalendarWorkSchedules;
using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;

/// <summary>
/// Конкретный рабочий день
/// </summary>
public class CalendarWorkScheduleItemDetails
{
    public Guid CalendarWorkScheduleID { get; init; }

    public CalendarDayType DayType { get; init; }

    public string DayTypeName { get; init; }

    public byte? ShiftNumber { get; init; }

    public int TimeSpanInMinutes { get; init; }

    public int TotalTimeSpanInMinutes { get; init; }

    public int TotalTimeSpanExclusionInMinutes { get; set; }

    public DateTime DayOfYearDate { get; init; } // DayOfYear.Date

    public int DayOfYear { get; init; }


    public string ExlusionName { get; init; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }
}
