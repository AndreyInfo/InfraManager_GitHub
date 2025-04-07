using System;

namespace InfraManager.BLL.Location.TimeZones;

public class TimeZoneAdjustmentRuleDetails
{
    public DateTime DateStart { get; init; }
    public DateTime DateEnd { get; init; }
    public short DaylightDeltaInMinutes { get; init; }
    public TimeZoneAdjustmentRuleTransitionDetails TransitionStart { get; init; }
    public TimeZoneAdjustmentRuleTransitionDetails TransitionEnd { get; init; }
}