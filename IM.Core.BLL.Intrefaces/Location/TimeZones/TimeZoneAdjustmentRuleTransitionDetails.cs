using System;

namespace InfraManager.BLL.Location.TimeZones;

public class TimeZoneAdjustmentRuleTransitionDetails
{
    public bool IsFixedDateRule { get; init; }
    public byte Month { get; init; }
    public byte? Day { get; init; }
    public DateTime TimeOfDay { get; init; }
    public byte? Week { get; init; }
    public byte? DayOfWeek { get; init; }
}