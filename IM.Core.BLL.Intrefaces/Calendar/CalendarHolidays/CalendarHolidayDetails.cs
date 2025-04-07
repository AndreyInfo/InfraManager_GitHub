using System;

namespace InfraManager.BLL.Calendar.CalendarHolidays;

public class CalendarHolidayDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? ComplementaryID { get; init; }

    public CalendarHolidayItemDetails[] CalendarHolidayItems { get; init; }
}