using System.Collections.Generic;
using InfraManager.Core;
using WorkOrderTemplate = InfraManager.BLL.Notification.Templates.WorkOrderTemplate;

namespace InfraManager.BLL.Notification.NotificationProviders;

internal class WorkOrderNotificationProvider : BaseNotificationProvider, INotificationProvider
{
    public WorkOrderNotificationProvider()
    {
        _prefix = "WorkOrder";
        _type = typeof(WorkOrderTemplate);
        
        _businessRoles.Add((int)BusinessRole.SDAdministrator, GetName(BusinessRole.SDAdministrator));
        _businessRoles.Add((int)BusinessRole.WorkOrderInitiator, GetName(BusinessRole.WorkOrderInitiator));
        _businessRoles.Add((int)BusinessRole.WorkOrderExecutor, GetName(BusinessRole.WorkOrderExecutor));
        _businessRoles.Add((int)BusinessRole.WorkOrderAssignor, GetName(BusinessRole.WorkOrderAssignor));
        _businessRoles.Add((int)BusinessRole.CallOwner, GetName(BusinessRole.CallOwner));
        _businessRoles.Add((int)BusinessRole.ProblemOwner, GetName(BusinessRole.ProblemOwner));
        _businessRoles.Add((int)BusinessRole.RFCOwner, GetName(BusinessRole.RFCOwner));
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