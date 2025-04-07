using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents.Events
{
    internal class MassIncidentEventWriterConfigurer : IConfigureEventWriter<MassIncident, MassIncident>
    {
        public void Configure(IEventWriter<MassIncident, MassIncident> writer)
        {
        }
    }
}
