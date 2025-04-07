using System.Collections.Generic;
using InfraManager.BLL.Notification.Templates;
using InfraManager.Core;

namespace InfraManager.BLL.Notification.NotificationProviders;

internal class NegotiationNotificationProvider : BaseNotificationProvider, INotificationProvider
{

    public NegotiationNotificationProvider()
    {
        _prefix = "Negotiation";
        _type = typeof(NegotiationTemplate);
        
        _businessRoles.Add((int)BusinessRole.SDAdministrator, GetName(BusinessRole.SDAdministrator));
        _businessRoles.Add((int)BusinessRole.NegotiationParticipant, GetName(BusinessRole.NegotiationParticipant));
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