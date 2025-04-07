using System;

namespace InfraManager.BLL.Calendar.CalendarHolidays;

public class CalendarHolidayInsertDetails
{
    public string Name { get; init; }
    public Guid? ComplementaryID { get; init; }

    public CalendarHolidayItemDetails[] CalendarHolidayItems { get; init; }
}
