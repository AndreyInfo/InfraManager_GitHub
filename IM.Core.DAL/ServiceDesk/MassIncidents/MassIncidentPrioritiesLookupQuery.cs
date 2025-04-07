using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class MassIncidentPrioritiesLookupQuery : ILookupQuery
    {
        private readonly DbSet<MassIncident> _massiveIncidents;
        private readonly DbSet<Priority> _priorities;

        public MassIncidentPrioritiesLookupQuery(DbSet<MassIncident> massiveIncidents, DbSet<Priority> priorities)
        {
            _massiveIncidents = massiveIncidents;
            _priorities = priorities;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var values = await _priorities
                .Where(p => _massiveIncidents.Any(massIncident => massIncident.PriorityID == p.ID))
                .ToArrayAsync(cancellationToken);
            return values
                .OrderBy(x => x.Sequence)
                .Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Name })
                .ToArray();
        }
    }
}
