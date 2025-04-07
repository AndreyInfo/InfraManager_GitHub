using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    internal class WorkOrderPrioritiesLookupQuery : ILookupQuery
    {
        private readonly DbSet<WorkOrder> _workOrders;
        private readonly DbSet<WorkOrderPriority> _workOrderPriority;

        public WorkOrderPrioritiesLookupQuery(DbSet<WorkOrder> workOrders, DbSet<WorkOrderPriority> workOrderPriority)
        {
            _workOrders = workOrders;
            _workOrderPriority = workOrderPriority;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from workOrders in _workOrders.AsNoTracking()
                        join priorities in _workOrderPriority.AsNoTracking()
                        on workOrders.PriorityID equals priorities.ID
                        select new
                        {
                            priorities.ID,
                            Info = priorities.Name
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}
