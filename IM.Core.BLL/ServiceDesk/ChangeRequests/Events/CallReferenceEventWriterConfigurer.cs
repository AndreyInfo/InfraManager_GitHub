using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests.Events
{
    internal class CallReferenceEventWriterConfigurer : IConfigureEventWriter<CallReference<ChangeRequest>, ChangeRequest>
    {
        public void Configure(IEventWriter<CallReference<ChangeRequest>, ChangeRequest> writer)
        {
        }
    }
}
