using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderPredicateBuilders : WorkOrderPredicateBuildersBase<WorkOrderListItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<WorkOrder, WorkOrderListItem>>
    { }
}
