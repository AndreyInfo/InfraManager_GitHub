using Inframanager;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentIsAvailableViaToz : 
        IBuildSpecification<MassIncident, User>, 
        ISelfRegisteredService<MassIncidentIsAvailableViaToz>
    {
        private readonly IMassIncidentTozQuery _query;

        public MassIncidentIsAvailableViaToz(IMassIncidentTozQuery query)
        {
            _query = query;
        }

        public Specification<MassIncident> Build(User filterBy)
        {
            var subquery = _query.Query(filterBy.IMObjID);

            return new Specification<MassIncident>(massIncident => subquery.Any(x => x.ID == massIncident.ID));
        }
    }
}
