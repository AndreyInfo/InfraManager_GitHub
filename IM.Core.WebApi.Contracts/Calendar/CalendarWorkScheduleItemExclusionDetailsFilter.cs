using System;

namespace InfraManager.WebApi.Contracts.Calendar;

public class CalendarWorkScheduleItemExclusionDetailsFilter
{
    public Guid WorkScheduleID { get; init; }
    public int? DayOfYear { get; init; }
}