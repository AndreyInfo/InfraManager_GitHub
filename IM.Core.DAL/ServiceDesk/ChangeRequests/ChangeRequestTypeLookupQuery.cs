using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestTypeLookupQuery : ILookupQuery
    {
        private readonly DbSet<ChangeRequest> _changeRequests;
        private readonly DbSet<ChangeRequestType> _changeRequestTypes;

        public ChangeRequestTypeLookupQuery(DbSet<ChangeRequest> changeRequests, DbSet<ChangeRequestType> changeRequestTypes)
        {
            _changeRequests = changeRequests;
            _changeRequestTypes = changeRequestTypes;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from changeRequest in _changeRequests.AsNoTracking()
                        join changeRequestType in _changeRequestTypes.AsNoTracking()
                        on changeRequest.RFCTypeID equals changeRequestType.ID
                        select new
                        {
                            changeRequestType.ID,
                            Info = changeRequestType.Name
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}
