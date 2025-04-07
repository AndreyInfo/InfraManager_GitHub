namespace InfraManager.BLL.Location.TimeZones;

public class TimeZoneDetails
{
    public string ID { get; init; }
    public string Name { get; init; }
    public short BaseUtcOffsetInMinutes { get; init; }
    public bool SupportsDaylightSavingTime { get; init; }
    public TimeZoneAdjustmentRuleDetails[] AdjustmentRules { get; init; } 
}