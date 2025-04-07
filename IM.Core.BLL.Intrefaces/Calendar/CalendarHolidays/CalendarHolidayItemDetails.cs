using System;
using InfraManager.DAL;

namespace InfraManager.BLL.Calendar.CalendarHolidays;

public class CalendarHolidayItemDetails
{
    public Guid ID { get; init; }
    public Guid CalendarHolidayID { get; init; }
    public byte Day { get; init; }
    public Month Month { get; init; }
}
