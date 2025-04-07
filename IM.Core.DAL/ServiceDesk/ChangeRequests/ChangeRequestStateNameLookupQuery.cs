using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestStateNameLookupQuery : ILookupQuery
    {
        private readonly DbSet<ChangeRequest> _changeRequests;

        public ChangeRequestStateNameLookupQuery(DbSet<ChangeRequest> changeRequests)
        {
            _changeRequests = changeRequests;
        }

        public Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return _changeRequests
                .Select(cr => cr.EntityStateName)
                .Distinct()
                .Select(stateName => new ValueData { ID = stateName, Info = stateName })
                .ToArrayAsync(cancellationToken);
        }
    }
}

