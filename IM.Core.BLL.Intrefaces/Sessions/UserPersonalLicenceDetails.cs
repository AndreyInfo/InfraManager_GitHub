using System;

namespace InfraManager.BLL.Sessions;

public class UserPersonalLicenceDetails
{
    public Guid UserID { get; init; }
    public DateTime UtcDateCreated { get; init; }
    
    public string FullName { get; init; }
    public string SubdivisionFullName { get; init; }
    public string Number { get; init; }
    public string LoginName { get; init; }
}