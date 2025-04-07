using System.Collections.Generic;
using InfraManager.BLL.Notification.Templates;
using InfraManager.Core;

namespace InfraManager.BLL.Notification.NotificationProviders;

internal class SubstitutionNotificationProvider : BaseNotificationProvider, INotificationProvider
{
    public SubstitutionNotificationProvider()
    {
        _prefix = "Substitution";
        _type = typeof(SubstitutionTemplate);
        
        _businessRoles.Add((int)BusinessRole.SDAdministrator, GetName(BusinessRole.SDAdministrator));
        _businessRoles.Add((int)BusinessRole.DeputyUser, GetName(BusinessRole.DeputyUser));
        _businessRoles.Add((int)BusinessRole.ReplacedUser, GetName(BusinessRole.ReplacedUser));
    }
    
    public Dictionary<int, string> GetRoles()
    {
        return _businessRoles;
    }

    public ParameterTemplate[] GetAvailableParameterList()
    {
        return BuildParameterTemplates();
    }
}