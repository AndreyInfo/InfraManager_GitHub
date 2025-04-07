using System;
using Inframanager.BLL.ListView;
using InfraManager.BLL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    [ListViewItem(ListView.MassIncidentReferencedWorkOrders, OperationID.MassIncident_Properties)]
    public class MassIncidentReferencedWorkOrderListItem : WorkOrderReferenceListItemBase
    {
        public Guid? ExecutorID { get; init; }
    }
}
