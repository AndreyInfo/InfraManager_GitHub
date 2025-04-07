using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class CallSlaLookupQuery : ILookupQuery
    {
        private readonly DbSet<Call> _calls;

        public CallSlaLookupQuery(DbSet<Call> calls)
        {
            _calls = calls;
        }

        public Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return _calls
                .Where(x => !x.Removed && x.SLAName != null)
                .Select(x => x.SLAName)
                .Distinct()
                .Select(slaName => new ValueData { ID = slaName, Info = slaName })
                .ToArrayAsync(cancellationToken);
        }
    }
}