using System;

namespace InfraManager.BLL.Sessions;

public class UserSessionHistoryDetails
{
    public Guid ID { get; init; }
    
    public string UserAgent { get; init; }
    
    public string TypeString { get; init; }
    
    public DateTime UtcDate { get; init; }
    
    public string UserFullName { get; init; }
    
    public Guid UserID { get; init; }
    
    public string ExecutorFullName { get; init; }
    
    public Guid? ExecutorID { get; init; }
}