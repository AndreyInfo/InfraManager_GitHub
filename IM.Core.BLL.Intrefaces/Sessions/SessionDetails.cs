using System;

namespace InfraManager.BLL.Sessions;

public class SessionDetails
{
    public Guid UserID { get; init; }
    
    public string SecurityStamp { get; init; }
    
    public DateTime UtcDateOpened { get; init; }
    
    public DateTime? UtcDateClosed { get; init; }
    
    public DateTime UtcDateLastActivity { get; init; }
    
    public string UserAgent { get; init; }
    
    public string UserName { get; init; }
    
    public string UserLogin { get; init; }
    
    public string UserSubdivisionFullName { get; init; }

    public string LocationName { get; init; }
    
    public string LicenceType { get; init; }

    public int DurationInMinutes => (int)(UtcDateClosed == null ? (DateTime.UtcNow - UtcDateOpened).TotalMinutes
        : (UtcDateClosed.Value - UtcDateOpened).TotalMinutes);
    
}