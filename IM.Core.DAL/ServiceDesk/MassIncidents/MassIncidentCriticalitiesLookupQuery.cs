using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class MassincidentCriticalitiesLookupQuery : ILookupQuery
    {
        private readonly DbSet<MassIncident> _massiveIncidents;
        private readonly DbSet<Criticality> _criticalities;

        public MassincidentCriticalitiesLookupQuery(DbSet<MassIncident> massiveIncidents, DbSet<Criticality> criticalities)
        {
            _massiveIncidents = massiveIncidents;
            _criticalities = criticalities;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var values = await _criticalities
                .Where(p => _massiveIncidents.Any(massIncident => massIncident.CriticalityID == p.ID))
                .ToArrayAsync(cancellationToken);
            return values
                .Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Name })
                .ToArray();
        }
    }
}
