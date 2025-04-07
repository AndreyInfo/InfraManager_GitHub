using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal class ReferencedProblemTypesLookupQuery : ILookupQuery
    {
        private readonly DbContext _db;

        public ReferencedProblemTypesLookupQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var problems = _db.Set<Problem>();
            var data = await _db.Set<ProblemType>()
                .Where(pt => problems.Any(p => p.Type.ID == pt.ID))
                .Select(pt => new { pt.ID, Info = ProblemType.GetFullProblemTypeName(pt.ID) })
                .ToArrayAsync();

            return data
                .Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Info })
                .ToArray();
        }
    }
}
