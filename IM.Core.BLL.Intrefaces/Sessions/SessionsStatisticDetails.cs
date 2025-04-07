namespace InfraManager.BLL.Sessions;

public class SessionsStatisticDetails
{
    public SessionsStatisticDetails(
        int activeEngineerSessionCount,
        int activePersonalSessionCount,
        int activeSessionsCount,
        int availableEngineerSessionCount,
        int availablePersonalSessionCount)
    {
        ActiveEngineerSessionCount = activeEngineerSessionCount;
        ActivePersonalSessionCount = activePersonalSessionCount;
        ActiveSessionsCount = activeSessionsCount;
        AvailableEngineerSessionCount = availableEngineerSessionCount;
        AvailablePersonalSessionCount = availablePersonalSessionCount;
    }
    
    public int ActiveEngineerSessionCount { get; init; }
    
    public int ActivePersonalSessionCount { get; init; }
    
    public int ActiveSessionsCount { get; init; }
    
    public int AvailableEngineerSessionCount { get; init; }
    
    public int AvailablePersonalSessionCount { get; init; }
}