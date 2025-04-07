using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.WorkOrderReferenced;

internal class ReferencedWorkOrderPredicateBuilders : WorkOrderPredicateBuildersBase<ReferencedWorkOrderListItem>,
    ISelfRegisteredService<IAggregatePredicateBuilders<WorkOrder, ReferencedWorkOrderListItem>>
{
    
}