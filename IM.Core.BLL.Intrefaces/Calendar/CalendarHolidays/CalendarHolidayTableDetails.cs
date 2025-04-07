using System;

namespace InfraManager.BLL.Calendar.CalendarHolidays;

public class CalendarHolidayTableDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid? ComplementaryID { get; init; }
}
