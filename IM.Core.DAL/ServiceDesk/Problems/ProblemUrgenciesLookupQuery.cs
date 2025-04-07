using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal class ProblemUrgenciesLookupQuery : ILookupQuery
    {
        private readonly DbContext _db;

        public ProblemUrgenciesLookupQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var problems = _db.Set<Problem>();
            var data = await _db.Set<Urgency>()
                .Where(x => problems.Any(p => p.Urgency.ID == x.ID))
                .Select(x => new { x.ID, Info = x.Name })
                .ToArrayAsync();

            return data
                .Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Info })
                .ToArray();
        }
    }
}
