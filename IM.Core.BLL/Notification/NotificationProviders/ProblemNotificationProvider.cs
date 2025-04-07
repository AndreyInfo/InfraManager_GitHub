using System.Collections.Generic;
using InfraManager.BLL.Notification.Templates;
using InfraManager.Core;

namespace InfraManager.BLL.Notification.NotificationProviders;

internal class ProblemNotificationProvider : BaseNotificationProvider, INotificationProvider
{
    public ProblemNotificationProvider()
    {
        _prefix = "Problem";
        _type = typeof(ProblemTemplate);
        
        _businessRoles.Add((int)BusinessRole.SDAdministrator, GetName(BusinessRole.SDAdministrator));
        _businessRoles.Add((int)BusinessRole.ProblemOwner, GetName(BusinessRole.ProblemOwner));
        _businessRoles.Add((int)BusinessRole.CallOwner, GetName(BusinessRole.CallOwner));
        _businessRoles.Add((int)BusinessRole.ProblemExecutor, GetName(BusinessRole.ProblemExecutor));
        _businessRoles.Add((int)BusinessRole.ProblemInitiator, GetName(BusinessRole.ProblemInitiator));
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