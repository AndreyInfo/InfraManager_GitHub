using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class CallServiceNameLookupQuery : ILookupQuery
    {
        private readonly DbSet<Call> _calls;
        private readonly DbSet<CallService> _callService;

        public CallServiceNameLookupQuery(DbSet<Call> calls, DbSet<CallService> callService)
        {
            _calls = calls;
            _callService = callService;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from call in _calls.AsNoTracking()
                        join service in _callService.AsNoTracking()
                        on call.CallServiceID equals service.ID
                        where call.CallServiceID != CallService.NullCallServiceID
                        select new
                        {
                            service.Service.ID,
                            Info = service.ServiceName
                        };

            return Array.ConvertAll(
                await query.Distinct().ToArrayAsync(cancellationToken),
                x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}
