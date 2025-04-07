using System.Collections.Generic;
using InfraManager.BLL.Notification.Templates;
using InfraManager.Core;

namespace InfraManager.BLL.Notification.NotificationProviders;

internal class CustomControllerNotificationProvider : BaseNotificationProvider, INotificationProvider
{
    public CustomControllerNotificationProvider()
    {
        _prefix = "CustomControl";
        _type = typeof(CustomControllerTemplate);
        
        _businessRoles.Add((int)BusinessRole.SDAdministrator, GetName(BusinessRole.SDAdministrator));
        _businessRoles.Add((int)BusinessRole.ControllerParticipant, GetName(BusinessRole.ControllerParticipant));
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