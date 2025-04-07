using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderLoader :
        ILoadEntity<Guid, WorkOrder>,
        ISelfRegisteredService<ILoadEntity<Guid, WorkOrder>>
    {
        private readonly IFindEntityByGlobalIdentifier<WorkOrder> _finder;

        public WorkOrderLoader(IFindEntityByGlobalIdentifier<WorkOrder> finder)
        {
            _finder = finder;
        }

        public Task<WorkOrder> LoadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _finder
                .With(x => x.Aggregate)
                .With(x => x.Assignee)
                .With(x => x.BudgetUsage)
                .With(x => x.BudgetUsageCause)
                .With(x => x.Executor)
                .With(x => x.Initiator)
                .With(x => x.Priority)
                .With(x => x.Group)
                .With(x => x.Type)
                .With(x => x.WorkOrderReference)
                .With(x => x.Manhours)
                .With(x => x.FinancePurchase)
                .FindOrRaiseErrorAsync(id, cancellationToken);
        }
    }
}
