using System;

namespace InfraManager.DAL.Sessions;

public class UserPersonalLicence 
{
    protected UserPersonalLicence()
    {
            
    }
    
    public UserPersonalLicence(Guid userID)
    {
        UserID = userID;
        UtcDateCreated = DateTime.UtcNow;
    }
    
    public int ID { get; }
    public Guid UserID { get; init; }
    public DateTime UtcDateCreated { get; }
    
    public virtual User User { get; }
}