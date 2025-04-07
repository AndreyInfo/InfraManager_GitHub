using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents.Events
{
    internal class AffectedServiceSubjectFinder : ISubjectFinder<ManyToMany<MassIncident, Service>, MassIncident>
    {
        private readonly IReadonlyRepository<MassIncident> _repository;

        public AffectedServiceSubjectFinder(IReadonlyRepository<MassIncident> repository)
        {
            _repository = repository;
        }

        public MassIncident Find(ManyToMany<MassIncident, Service> entity, IEntityState originalState)
        {
            return entity.Parent;
        }
    }
}
