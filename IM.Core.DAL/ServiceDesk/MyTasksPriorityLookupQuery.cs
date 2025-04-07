using InfraManager;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    internal class MyTasksPriorityLookupQuery : ILookupQuery
    {
        private readonly DbContext _db;

        public MyTasksPriorityLookupQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var data = await _db.Set<Priority>()
                .Where(p => _db.Set<Call>().Any(call => call.PriorityID == p.ID)
                    || _db.Set<Problem>().Any(problem => problem.PriorityID == p.ID)
                    || _db.Set<MassIncident>().Any(massIncident => massIncident.PriorityID == p.ID))
                .Select(p => new { p.ID, p.Name })
                .Union(
                    _db.Set<WorkOrderPriority>()
                        .Where(p => _db.Set<WorkOrder>().Any(wo => wo.PriorityID == p.ID))
                        .Select(p => new { p.ID, p.Name }))
                .ToArrayAsync(cancellationToken);

            return data.Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Name }).ToArray();
        }
    }
}

