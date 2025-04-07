using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests.Events
{
    internal class ChangeRequestEventWriterConfigurer : 
        IConfigureEventWriter<ChangeRequest, ChangeRequest>,
        ISelfRegisteredService<IConfigureEventWriter<ChangeRequest, ChangeRequest>>
    {
        public void Configure(IEventWriter<ChangeRequest, ChangeRequest> writer)
        {
        }
    }
}
