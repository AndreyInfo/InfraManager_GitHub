using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    internal class NegotiationPriorityLookupQuery : ILookupQuery
    {
        private readonly DbSet<WorkOrder> _workOrders;
        private readonly DbSet<Call> _calls;
        private readonly DbSet<Problem> _problems;

        public NegotiationPriorityLookupQuery(
            DbSet<WorkOrder> workOrders,
            DbSet<Call> calls,
            DbSet<Problem> problems)
        {
            _workOrders = workOrders;
            _calls = calls;
            _problems = problems;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var callQuery = from calls in _calls.AsNoTracking()
                            where !calls.Removed //TODO: Consider whether to define QueryFilter for entity Call at ef configuration level
                            select new
                            {
                                calls.Priority.ID,
                                Info = calls.Priority.Name
                            };

            var workOrderQuery = from workOrders in _workOrders.AsNoTracking()
                                 select new
                                 {
                                     workOrders.Priority.ID,
                                     Info = workOrders.Priority.Name
                                 };

            var problemQuery = from problems in _problems.AsNoTracking()
                               select new
                               {
                                   problems.Priority.ID,
                                   Info = problems.Priority.Name
                               };

            var query = callQuery.Distinct()
                .Union(workOrderQuery.Distinct())
                .Union(problemQuery.Distinct())
                .ToArrayAsync(cancellationToken);

            return Array.ConvertAll(await query, x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}

