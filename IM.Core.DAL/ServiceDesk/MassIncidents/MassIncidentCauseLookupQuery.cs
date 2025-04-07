using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class MassIncidentCauseLookupQuery : ILookupQuery
    {
        private readonly DbSet<MassIncidentCause> _set;

        public MassIncidentCauseLookupQuery(DbSet<MassIncidentCause> set)
        {
            _set = set;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var data = await _set.AsNoTracking().ToArrayAsync(cancellationToken);

            return data.Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Name }).ToArray();
        }
    }
}
