using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.DeleteStrategies
{
    internal class MassiceIncidentCallDependencyDeleteStrategy :
        IDependentDeleteStrategy<Call>,
        ISelfRegisteredService<IDependentDeleteStrategy<Call>>
    {
        private readonly DbSet<MassIncident> _massIncidents;

        public MassiceIncidentCallDependencyDeleteStrategy(DbSet<MassIncident> massIncidents)
        {
            _massIncidents = massIncidents;
        }

        public void OnDelete(Call entity)
        {
            foreach(var affectedMassIncident in _massIncidents
                .Include(mi => mi.Calls)
                .ThenInclude(x => x.Reference)
                .Where(mi => mi.Calls.Any(x => x.Reference.IMObjID == entity.IMObjID))
                .ToArray())
            {
                var reference = affectedMassIncident.Calls.First(x => x.Reference.IMObjID == entity.IMObjID);
                affectedMassIncident.Calls.Remove(reference);
            }
        }
    }
}
