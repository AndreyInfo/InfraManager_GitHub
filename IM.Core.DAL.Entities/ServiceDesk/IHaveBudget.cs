using System;

namespace InfraManager.DAL.ServiceDesk
{
    public interface IHaveBudget
    {
        Guid BudgetUsageAggregateID { get; }
        Guid BudgetUsageCauseAggregateID { get; }
    }
}
