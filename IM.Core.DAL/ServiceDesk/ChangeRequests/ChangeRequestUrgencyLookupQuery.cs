using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestUrgencyLookupQuery : ILookupQuery
    {
        private readonly DbSet<ChangeRequest> _changeRequests;
        private readonly DbSet<Urgency> _urgencies;

        public ChangeRequestUrgencyLookupQuery(DbSet<ChangeRequest> changeRequests, DbSet<Urgency> urgencies)
        {
            _changeRequests = changeRequests;
            _urgencies = urgencies;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from changeRequest in _changeRequests.AsNoTracking()
                        join urgency in _urgencies.AsNoTracking()
                        on changeRequest.UrgencyID equals urgency.ID
                        select new
                        {
                            urgency.ID,
                            Info = urgency.Name
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}
