using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class CallUrgenciesLookupQuery : ILookupQuery
    {
        private readonly DbSet<Call> _calls;
        private readonly DbSet<Urgency> _urgency;

        public CallUrgenciesLookupQuery(DbSet<Call> calls, DbSet<Urgency> urgency)
        {
            _calls = calls;
            _urgency = urgency;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from calls in _calls.AsNoTracking()
                        join urgencies in _urgency.AsNoTracking()
                        on calls.UrgencyID equals urgencies.ID
                        where !calls.Removed
                        select new
                        {
                            urgencies.ID,
                            Info = urgencies.Name
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}