using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class CallInfluencesLookupQuery : ILookupQuery
    {
        private readonly DbSet<Call> _calls;
        private readonly DbSet<Influence> _influence;

        public CallInfluencesLookupQuery(DbSet<Call> calls, DbSet<Influence> influence)
        {
            _calls = calls;
            _influence = influence;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from calls in _calls.AsNoTracking()
                        join influences in _influence.AsNoTracking()
                        on calls.InfluenceID equals influences.ID
                        where !calls.Removed
                        select new
                        {
                            influences.ID,
                            Info = influences.Name
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}