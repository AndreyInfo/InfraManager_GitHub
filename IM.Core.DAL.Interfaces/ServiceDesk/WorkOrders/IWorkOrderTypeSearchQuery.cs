using System;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    public class WorkOrderTypeSearchCriteria : SearchCriteria
    {
        public WorkOrderTypeClass? TypeClass { get; init; } // TODO: enum
    }

    public interface IWorkOrderTypeSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(
            Guid currentUserId, 
            WorkOrderTypeSearchCriteria searchBy);
    }
}
