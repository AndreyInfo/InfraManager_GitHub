using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentLoader : 
        ILoadEntity<int, MassIncident>,
        ISelfRegisteredService<ILoadEntity<int, MassIncident>>
    {
        private readonly IReadonlyRepository<MassIncident> _finder; 

        public MassIncidentLoader(IReadonlyRepository<MassIncident> finder)
        {
            _finder = finder;
        }

        public async Task<MassIncident> LoadAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _finder
                .With(x => x.CreatedBy)
                .With(x => x.Priority)
                .With(x => x.OwnedBy)
                .With(x => x.ExecutedByUser)
                .With(x => x.Service)
                .With(x => x.ExecutedByGroup)
                    .ThenWith(x => x.QueueUsers)
                .WithMany(x => x.AffectedServices)
                    .ThenWith(x => x.Reference)
                .SingleOrDefaultAsync(x => x.ID == id, cancellationToken) 
                   ?? throw new ObjectNotFoundException<int>(id, ObjectClass.MassIncident);
        }
    }
}
