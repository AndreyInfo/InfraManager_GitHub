using System.Collections.Generic;
using InfraManager.BLL.Notification.Templates;
using InfraManager.Core;

namespace InfraManager.BLL.Notification.NotificationProviders;

internal class ChangeRequestNotificationProvider : BaseNotificationProvider, INotificationProvider
{
    public ChangeRequestNotificationProvider()
    {
        _prefix = "RFC";
        _type = typeof(RFCTemplate);
        
        _businessRoles.Add((int)BusinessRole.SDAdministrator, GetName(BusinessRole.SDAdministrator));
        _businessRoles.Add((int)BusinessRole.RFCOwner, GetName(BusinessRole.RFCOwner));
        _businessRoles.Add((int)BusinessRole.RFCInitiator, GetName(BusinessRole.RFCInitiator));
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