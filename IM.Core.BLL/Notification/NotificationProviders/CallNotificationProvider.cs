using System.Collections.Generic;
using InfraManager.BLL.Notification.Templates;
using InfraManager.Core;

namespace InfraManager.BLL.Notification.NotificationProviders;

internal class CallNotificationProvider : BaseNotificationProvider, INotificationProvider
{
    public CallNotificationProvider()
    {
        _prefix = "Call";
        _type = typeof(CallTemplate);
        
        _businessRoles.Add((int)BusinessRole.SDAdministrator, GetName(BusinessRole.SDAdministrator));
        _businessRoles.Add((int)BusinessRole.CallOwner, GetName(BusinessRole.CallOwner));
        _businessRoles.Add((int)BusinessRole.CallInitiator, GetName(BusinessRole.CallInitiator));
        _businessRoles.Add((int)BusinessRole.CallExecutor, GetName(BusinessRole.CallExecutor));
        _businessRoles.Add((int)BusinessRole.CallClient, GetName(BusinessRole.CallClient));
        _businessRoles.Add((int)BusinessRole.ProblemOwner, GetName(BusinessRole.ProblemOwner));
        _businessRoles.Add((int)BusinessRole.CallAccomplisher, GetName(BusinessRole.CallAccomplisher));
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