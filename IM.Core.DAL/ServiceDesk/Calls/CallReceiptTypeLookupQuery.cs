using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class CallReceiptTypeLookupQuery : ILookupQuery
    {
        private readonly DbSet<Call> _calls;

        public CallReceiptTypeLookupQuery(DbSet<Call> calls)
        {
            _calls = calls;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = _calls
                .Where(x => !x.Removed)
                .Select(x => x.ReceiptType)
                .Distinct()
                .ToArrayAsync(cancellationToken);

            return Array.ConvertAll(await query, x => new ValueData { ID = x.ToString(), Info = x.ToString() });
        }
    }
}
