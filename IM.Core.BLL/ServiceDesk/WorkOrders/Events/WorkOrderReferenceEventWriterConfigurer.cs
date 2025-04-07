using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Events
{
    internal class WorkOrderReferenceEventWriterConfigurer : IConfigureEventWriter<WorkOrder, WorkOrderReference>
    {
        public void Configure(IEventWriter<WorkOrder, WorkOrderReference> writer)
        {
        }
    }
}
