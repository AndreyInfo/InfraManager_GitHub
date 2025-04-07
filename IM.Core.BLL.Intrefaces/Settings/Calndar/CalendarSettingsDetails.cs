using System;

namespace InfraManager.BLL.Settings.Calendar;

public sealed class CalendarSettingsDetails
{
    public Guid? CalendarHolidayID { get; init; }
    public string CalendarHolidayName { get; init; }
    public Guid? CalendarWeekendID { get; init; }
    public string CalendarWeekendName { get; init; }
    public string TimeZoneID { get; init; }
    public string TimeZoneName { get; init; }
    public DateTime TimeStart { get; init; }
    public DateTime TimeStartEnd { get; init; }
    public DateTime TimeEnd { get; init; }
    public short TimeSpanInMinutes { get => (short)(TimeStartEnd - TimeStart).TotalMinutes; }
    public byte[] RowVersion { get; init; }
    public DateTime? DinnerTimeStart { get; init; }
    public DateTime? DinnerTimeEnd { get; init; }
    public short? DinnerTimeSpanInMinutes 
    { 
        get => DinnerTimeEnd.HasValue && DinnerTimeStart.HasValue
                ? (short)(DinnerTimeEnd.Value - DinnerTimeStart.Value).TotalMinutes
                : null;
    }
    public bool AllowDinner { get => DinnerTimeEnd.HasValue && DinnerTimeStart.HasValue; }
}
