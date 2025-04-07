using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestPriorityLookupQuery : ILookupQuery
    {
        private readonly DbSet<ChangeRequest> _changeRequests;
        private readonly DbSet<Priority> _priorities;

        public ChangeRequestPriorityLookupQuery(DbSet<ChangeRequest> changeRequests, DbSet<Priority> priorities)
        {
            _changeRequests = changeRequests;
            _priorities = priorities;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from changeRequest in _changeRequests.AsNoTracking()
                        join priority in _priorities.AsNoTracking()
                        on changeRequest.PriorityID equals priority.ID
                        select new
                        {
                            priority.ID,
                            Info = priority.Name
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}
