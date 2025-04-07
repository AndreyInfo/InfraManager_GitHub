using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    //TODO: Убрать этот класс, т.к. параметры фильтрации должны приходить с клиента, т.о. возвращаться должен только WorkOrderQuery
    internal class InventoryListQuery : IListQuery<WorkOrder, InventoryListQueryResultItem>, ISelfRegisteredService<IListQuery<WorkOrder, InventoryListQueryResultItem>>
    {
        IWorkOrderListQueryBase <InventoryListQueryResultItem> _inventoriesQueryBase;

        public InventoryListQuery(IWorkOrderListQueryBase<InventoryListQueryResultItem> inventoriesQueryBase)
        {
            _inventoriesQueryBase = inventoriesQueryBase;
        }
        
        public IQueryable<InventoryListQueryResultItem> Query(Guid userID, IEnumerable<Expression<Func<WorkOrder, bool>>> filterBy)
        {
            return _inventoriesQueryBase.Query(userID, filterBy.Append(workOrder => workOrder.Type.TypeClass == WorkOrderTypeClass.Inventorization));
        }
    }
}

