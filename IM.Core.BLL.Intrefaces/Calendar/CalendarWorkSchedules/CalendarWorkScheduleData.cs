using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules;

public class CalendarWorkScheduleData
{
    public string Name { get; init; }
    public string Note { get; init; }
    public int Year { get; init; }
    public string ShiftTemplate { get; init; }
    public byte ShiftTemplateLeft { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? CalendarHolidayID { get; init; }
    public Guid? CalendarWeekendID { get; init; }
}
