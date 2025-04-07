using InfraManager;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Finance;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    internal class WorkOrderBudgetUsageCauseLookupQuery : ILookupQuery
    {
        private readonly DbContext _db;

        public WorkOrderBudgetUsageCauseLookupQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {

            var budgetUsageCauses = await
                (from workOrder in _db.Set<WorkOrder>().AsNoTracking()
                 join budgetUsageCause in _db.Set<BudgetUsageCause>().AsNoTracking()
                     on new { ID = workOrder.IMObjID, classID = ObjectClass.WorkOrder }
                         equals new { ID = budgetUsageCause.ObjectId, classID = budgetUsageCause.ObjectClass }
                 where budgetUsageCause.SLAID == null
                 select new ValueData
                 {
                     ID = budgetUsageCause.Text,
                     Info = budgetUsageCause.Text
                 }).ToArrayAsync(cancellationToken);

            var slaList = await (from workOrder in _db.Set<WorkOrder>().AsNoTracking()
                                 join budgetUsageCause in _db.Set<BudgetUsageCause>().AsNoTracking()
                                     on new { ID = workOrder.IMObjID, classID = ObjectClass.WorkOrder }
                                         equals new { ID = budgetUsageCause.ObjectId, classID = budgetUsageCause.ObjectClass }
                                 join sla in _db.Set<ServiceLevelAgreement>().AsNoTracking()
                                     on budgetUsageCause.SLAID equals sla.ID
                                         into slaBudgetUsageCause
                                 from sla in slaBudgetUsageCause.DefaultIfEmpty()
                                 where budgetUsageCause.SLAID != null
                                 select new
                                 {
                                     ID = budgetUsageCause.SLAID,
                                     Info = sla.Name
                                 }).ToArrayAsync(cancellationToken);

            return budgetUsageCauses.Distinct()
                .Union(slaList.Select(x => new ValueData { ID = x.ID.ToString(), Info = x.Info }))
                .ToArray();
        }
    }
}

