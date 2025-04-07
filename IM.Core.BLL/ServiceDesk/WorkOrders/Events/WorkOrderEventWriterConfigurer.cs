using Inframanager.BLL.Events;
using InfraManager.Core.Readers;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Events
{
    internal class WorkOrderEventWriterConfigurer : IConfigureEventWriter<WorkOrder, WorkOrder>
    {
        public void Configure(IEventWriter<WorkOrder, WorkOrder> writer)
        { 
        }
    }
}
