using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class CallPrioritiesLookupQuery : ILookupQuery
    {
        private readonly DbSet<Call> _calls;
        private readonly DbSet<Priority> _priority;

        public CallPrioritiesLookupQuery(DbSet<Call> calls, DbSet<Priority> priority)
        {
            _calls = calls;
            _priority = priority;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from calls in _calls.AsNoTracking()
                        join priorities in _priority.AsNoTracking()
                        on calls.Priority.ID equals priorities.ID
                        where !calls.Removed
                        select new
                        {
                            priorities.ID,
                            Info = priorities.Name
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}