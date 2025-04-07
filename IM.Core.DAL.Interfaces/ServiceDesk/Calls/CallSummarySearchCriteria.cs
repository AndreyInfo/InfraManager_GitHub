using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    public class CallSummarySearchCriteria : SearchCriteria
    {
        public Guid CallTypeID { get; init; }
    }
}
