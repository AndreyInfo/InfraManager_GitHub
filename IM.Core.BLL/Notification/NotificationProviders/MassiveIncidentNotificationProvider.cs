using System.Collections.Generic;
using InfraManager.BLL.Notification.Templates;
using InfraManager.Core;

namespace InfraManager.BLL.Notification.NotificationProviders;

internal class MassiveIncidentNotificationProvider : BaseNotificationProvider, INotificationProvider
{
    public MassiveIncidentNotificationProvider()
    {
        _prefix = "MassIncident";
        _type = typeof(MassIncidentTemplate);
        
        _businessRoles.Add((int)BusinessRole.SDAdministrator, GetName(BusinessRole.SDAdministrator));
        _businessRoles.Add((int)BusinessRole.MassIncidentInitiator, GetName(BusinessRole.MassIncidentInitiator));
        _businessRoles.Add((int)BusinessRole.MassIncidentExecutor, GetName(BusinessRole.MassIncidentExecutor));
        _businessRoles.Add((int)BusinessRole.MassIncidentOwner, GetName(BusinessRole.MassIncidentOwner));
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