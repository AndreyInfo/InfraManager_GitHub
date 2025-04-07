using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    internal class NegotiationStateNameLookupQuery : ILookupQuery
    {
        private readonly DbSet<WorkOrder> _workOrders;
        private readonly DbSet<Call> _calls;
        private readonly DbSet<Problem> _problems;

        public NegotiationStateNameLookupQuery(
            DbSet<WorkOrder> workOrders,
            DbSet<Call> calls,
            DbSet<Problem> problems)
        {
            _workOrders = workOrders;
            _calls = calls;
            _problems = problems;
        }

        public Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return _calls
                .Where(c => !c.Removed)
                .Select(c => c.EntityStateName)
                .Distinct()
                .Select(stateName => new ValueData { ID = DbFunctions.CastAsString(stateName), Info = DbFunctions.CastAsString(stateName) })
                .Union(_problems
                .Select(p => p.EntityStateName)
                .Distinct()
                .Select(stateName => new ValueData { ID = DbFunctions.CastAsString(stateName), Info = DbFunctions.CastAsString(stateName) }))
                .Union(_workOrders
                .Select(w => w.EntityStateName)
                .Distinct()
                .Select(stateName => new ValueData { ID = DbFunctions.CastAsString(stateName), Info = DbFunctions.CastAsString(stateName) }))
                .ToArrayAsync(cancellationToken);
        }
    }
}

