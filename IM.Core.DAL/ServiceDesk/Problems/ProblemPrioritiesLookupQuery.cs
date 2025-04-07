using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal class ProblemPrioritiesLookupQuery : ILookupQuery
    {
        private readonly DbSet<Problem> _problems;
        private readonly DbSet<Priority> _priorities;

        public ProblemPrioritiesLookupQuery(DbSet<Problem> problems, DbSet<Priority> priorities)
        {
            _problems = problems;
            _priorities = priorities;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var data = await _priorities
                .Where(x => _problems.Any(p => p.Priority.ID == x.ID))
                .Select(x => new { x.ID, x.Name })
                .ToArrayAsync();

            return data.Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Name }).ToArray();
        }
    }
}
