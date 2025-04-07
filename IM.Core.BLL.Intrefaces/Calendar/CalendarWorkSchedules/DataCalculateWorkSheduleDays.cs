using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules;

public class DataCalculateWorkSheduleDays
{
    public int ShiftTemplateLeft { get; init; }

    public string ShiftTemplate { get; init; }

    public Guid? CalendarHolidayID { get; init; }

    public Guid? CalendarWeekendID { get; init; }
}
