using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class CallTypeLookupQuery : ILookupQuery
    {
        private readonly DbSet<Call> _calls;

        public CallTypeLookupQuery(DbSet<Call> calls)
        {
            _calls = calls;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from calls in _calls.AsNoTracking()
                        where !calls.Removed
                        select new
                        {
                            calls.CallType.ID,
                            Info = CallType.GetFullCallTypeName(calls.CallType.ID)
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}

