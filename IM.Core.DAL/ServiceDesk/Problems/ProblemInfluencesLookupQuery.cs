using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal class ProblemInfluencesLookupQuery : ILookupQuery
    {
        private readonly CrossPlatformDbContext _db;

        public ProblemInfluencesLookupQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var problems = _db.Set<Problem>();
            var data = await _db.Set<Influence>()
                .Where(x => problems.Any(p => p.Influence.ID == x.ID))
                .Select(x => new { x.ID, x.Name })
                .ToArrayAsync();

            return data.Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Name }).ToArray();
        }
    }
}
