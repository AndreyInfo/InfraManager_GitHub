using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    internal class WorkOrderTypeLookupQuery : ILookupQuery
    {
        private readonly DbSet<WorkOrder> _workOrders;
        private readonly DbSet<WorkOrderType> _workOrderType;

        public WorkOrderTypeLookupQuery(DbSet<WorkOrder> workOrders, DbSet<WorkOrderType> workOrderType)
        {
            _workOrders = workOrders;
            _workOrderType = workOrderType;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from workOrders in _workOrders.AsNoTracking()
                        join workOrderTypes in _workOrderType.AsNoTracking()
                        on workOrders.TypeID equals workOrderTypes.ID
                        select new
                        {
                            workOrderTypes.ID,
                            Info = workOrderTypes.Name
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}
