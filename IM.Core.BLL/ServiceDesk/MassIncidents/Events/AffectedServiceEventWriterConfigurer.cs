using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents.Events
{
    internal class AffectedServiceEventWriterConfigurer : IConfigureEventWriter<ManyToMany<MassIncident, Service>, MassIncident>
    {
        public void Configure(IEventWriter<ManyToMany<MassIncident, Service>, MassIncident> writer)
        {
            // нет специфических настроек
        }
    }
}
