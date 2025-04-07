using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class MassIncidentServicesLookupQuery : ILookupQuery
    {
        private readonly DbSet<MassIncident> _massIncidents;
        private readonly DbSet<Service> _services;

        public MassIncidentServicesLookupQuery(DbSet<MassIncident> massIncidents, DbSet<Service> services)
        {
            _massIncidents = massIncidents;
            _services = services;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var values = await _services
                .Where(s => _massIncidents.Any(mi => mi.ServiceID == s.ID))
                .Select(s => new { s.ID, ServiceName = s.Name, })
                .ToArrayAsync(cancellationToken);

            return values.Select(x => new ValueData { ID = x.ID.ToString(), Info = x.ServiceName, }).ToArray();
        }
    }
}
