using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestInfluenceLookupQuery : ILookupQuery
    {
        private readonly DbSet<ChangeRequest> _changeRequests;
        private readonly DbSet<Influence> _influencies;

        public ChangeRequestInfluenceLookupQuery(DbSet<ChangeRequest> changeRequests, DbSet<Influence> influencies)
        {
            _changeRequests = changeRequests;
            _influencies = influencies;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from changeRequest in _changeRequests.AsNoTracking()
                        join influency in _influencies.AsNoTracking()
                        on changeRequest.InfluenceID equals influency.ID
                        select new
                        {
                            influency.ID,
                            Info = influency.Name
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}
