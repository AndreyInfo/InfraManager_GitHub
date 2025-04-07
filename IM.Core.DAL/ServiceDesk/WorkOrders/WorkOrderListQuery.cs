using InfraManager;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    internal class WorkOrderListQuery : IWorkOrderDataProvider, ISelfRegisteredService<IWorkOrderDataProvider>,
        IListQuery<WorkOrder, WorkOrderListQueryResultItem>,
        ISelfRegisteredService<IListQuery<WorkOrder, WorkOrderListQueryResultItem>>
    {
        private readonly CrossPlatformDbContext _db;
        IWorkOrderListQueryBase<WorkOrderListQueryResultItem> _workOrderQueryBase;

        public WorkOrderListQuery(CrossPlatformDbContext db, IWorkOrderListQueryBase<WorkOrderListQueryResultItem> workOrderQueryBase)
        {
            _db = db;
            _workOrderQueryBase =  workOrderQueryBase;
        }

        public async Task UpdateToDefultPriorityByPriorityIdAsync(Guid priorityId, Guid defultPriorityId)
        {
            IQueryable<WorkOrder> query = _db.Set<WorkOrder>();

            var result = await query.Where(c => !c.IsFinished && c.PriorityID == priorityId)
                                    .ToListAsync();

            foreach (var item in result)
            {
                item.PriorityID = defultPriorityId;
            }
            await _db.SaveChangesAsync();
        }

        public IQueryable<WorkOrderListQueryResultItem> Query(Guid userID, IEnumerable<Expression<Func<WorkOrder, bool>>> filterBy)
        {
            return _workOrderQueryBase.Query(userID, filterBy);
        }
    }
}

