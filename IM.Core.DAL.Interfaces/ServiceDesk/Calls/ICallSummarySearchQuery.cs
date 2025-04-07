using System;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    public interface ICallSummarySearchQuery
    {
        IQueryable<ObjectSearchResult> Query(CallSummarySearchCriteria searchCriteria, Guid userId);
    }
}
