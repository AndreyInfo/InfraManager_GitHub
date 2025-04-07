using System;

namespace InfraManager.DAL;

public class CalendarHolidayItem
{
    public Guid ID { get; init; }
    public Guid CalendarHolidayID { get; set; }
    public byte Day { get; init; }
    public Month Month { get; init; }
}
