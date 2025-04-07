using System;

namespace InfraManager.DAL.Sessions;

public class SessionDetailsListItem
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

    public SessionLocationType Location { get; init; }
    
    public SessionLicenceType LicenceType { get; init; }
}