using System;

namespace InfraManager.DAL.Sessions;

public class Session
{
    protected  Session()
    {
        
    }
    
    public Session(Guid userID,
        string userAgent,
        string securityStamp,
        SessionLocationType locationType,
        SessionLicenceType licenceType = SessionLicenceType.Concurrency)
    {
        UserID = userID;
        UserAgent = userAgent;
        SecurityStamp = securityStamp;
        UtcDateOpened = DateTime.UtcNow;
        UtcDateLastActivity = UtcDateOpened;
        LicenceType = licenceType;
        Location = locationType;
    }

    public Guid UserID { get; init; }
    public string SecurityStamp { get; set; }
    public DateTime UtcDateOpened { get; set; }
    public DateTime? UtcDateClosed { get; set; }
    public DateTime UtcDateLastActivity { get; set; }
    public string UserAgent { get; init; }
    public SessionLicenceType LicenceType { get; set; }
    public SessionLocationType Location { get; set; } 
    
    public virtual User User { get; init; }
}