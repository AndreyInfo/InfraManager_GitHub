using System;

namespace InfraManager.DAL.Sessions;

public class UserSessionHistory
{
    protected UserSessionHistory()
    {
        
    }
    
    public UserSessionHistory(
        SessionHistoryType type,
        Guid userID,
        string userAgent,
        Guid? executorID = null)
    {
        UtcDate = DateTime.UtcNow;
        Type = type;
        UserID = userID;
        UserAgent = userAgent;
        ExecutorID = executorID;
    }
    
    public Guid ID { get; }
    public SessionHistoryType Type { get; }
    public DateTime UtcDate { get; }
    public Guid UserID { get; }
    public Guid? ExecutorID { get; }
    public string UserAgent { get; }
    
    public virtual User User { get; }
    public virtual User Executor { get; }
}