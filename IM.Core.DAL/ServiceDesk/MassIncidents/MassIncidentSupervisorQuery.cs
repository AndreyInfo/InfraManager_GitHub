using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class MassIncidentSupervisorQuery : ISupervisorQuery<MassIncident>, ISelfRegisteredService<ISupervisorQuery<MassIncident>>
    {
        private readonly DbSet<MassIncident> _massIncidents;

        public MassIncidentSupervisorQuery(DbSet<MassIncident> massIncidents)
        {
            _massIncidents = massIncidents;
        }

        public IQueryable<MassIncident> Query(User user)
        {
            var subdivisionID = user.SubdivisionID;
            return _massIncidents.Where(
                massIncident => Subdivision.SubdivisionIsSibling(subdivisionID, massIncident.CreatedBy.SubdivisionID)
                    || Subdivision.SubdivisionIsSibling(subdivisionID, massIncident.OwnedBy.SubdivisionID)
                    || Subdivision.SubdivisionIsSibling(subdivisionID, massIncident.ExecutedByUser.SubdivisionID));
        }
    }
}
