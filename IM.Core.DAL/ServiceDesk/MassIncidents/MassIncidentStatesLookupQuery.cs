using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class MassIncidentStatesLookupQuery : ILookupQuery
    {
        private readonly DbSet<MassIncident> _set;

        public MassIncidentStatesLookupQuery(DbSet<MassIncident> set)
        {
            _set = set;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var values = await _set.Select(x => new { x.EntityStateID, x.EntityStateName }).Distinct().ToArrayAsync(cancellationToken);
            return values.Select(x => new ValueData { ID = x.EntityStateID, Info = x.EntityStateName }).ToArray();
        }
    }
}
