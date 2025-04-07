using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentCauseLoader :
        ILoadEntity<int, MassIncidentCause, MassIncidentCauseDetails>,
        IBuildEntityQuery<MassIncidentCause, MassIncidentCauseDetails, MassIncidentCauseListFilter>,
        ISelfRegisteredService<ILoadEntity<int, MassIncidentCause, MassIncidentCauseDetails>>,
        ISelfRegisteredService<IBuildEntityQuery<MassIncidentCause, MassIncidentCauseDetails, MassIncidentCauseListFilter>>
    {
        private IReadonlyRepository<MassIncidentCause> _repository;

        public MassIncidentCauseLoader(IReadonlyRepository<MassIncidentCause> repository)
        {
            _repository = repository;
        }

        public async Task<MassIncidentCause> LoadAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _repository.SingleOrDefaultAsync(x => x.ID == id, cancellationToken);
        }

        public IExecutableQuery<MassIncidentCause> Query(MassIncidentCauseListFilter filterBy)
        {
            var query = _repository.Query();

            if(filterBy.GlobalIdentifiers.HasValue)
            {
                query = query.Where(x=>x.IMObjID==filterBy.GlobalIdentifiers.Value);
            }

            return query;
        }

    }
}
