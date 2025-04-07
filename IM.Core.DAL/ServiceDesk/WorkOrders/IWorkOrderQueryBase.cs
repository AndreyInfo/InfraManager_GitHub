using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    internal interface IWorkOrderListQueryBase<T> where T : WorkOrderListQueryResultItemBase, new()
    {
        public IQueryable<T> Query(Guid userID, IEnumerable<Expression<Func<WorkOrder, bool>>> filterBy);
    }
}
