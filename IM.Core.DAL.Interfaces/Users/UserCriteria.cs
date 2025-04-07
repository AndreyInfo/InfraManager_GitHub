using System.Collections;
using System.Collections.Generic;

namespace InfraManager.DAL.Users;

public class UserCriteria
{
    public IEnumerable<FIO> FIOs { get; init; }
    
    public IEnumerable<NameSurname> NameSurnames { get; init; }
    
    public IEnumerable<string> Emails { get; init; }
    
    public IEnumerable<string> Numbers { get; init; }
    
    public IEnumerable<string> Logins { get; init; }
    
    public IEnumerable<string> ExternalIDs { get; init; }
    
    public IEnumerable<string> SIDs { get; init; }
    
    public bool WithRemoved { get; init; }

}