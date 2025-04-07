using System;

namespace InfraManager.DAL;

public class CalendarWorkScheduleDefault
{
    public Guid ID { get; init; }
    public Guid? CalendarHolidayID { get; init; }
    public Guid? CalendarWeekendID { get; init; }
    public string TimeZoneID { get; init; }
    public DateTime TimeStart { get; init; }
    public DateTime TimeEnd { get; init; }
    public short TimeSpanInMinutes { get; init; }
    public byte[] RowVersion { get; init; }
    public DateTime? DinnerTimeStart { get; init; }
    public DateTime? DinnerTimeEnd { get; init; }
    public short? ExclusionTimeSpanInMinutes { get; init; }

    public virtual CalendarHoliday CalendarHoliday { get; init; }
    public virtual CalendarWeekend CalendarWeekend { get; init; }
    public virtual ServiceDesk.TimeZone TimeZone { get; init; }

}
