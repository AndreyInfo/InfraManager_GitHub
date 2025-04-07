using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.WorkOrderReferenced;

internal class ReferencedWorkOrderPredicateBuilder : FilterBuildersAggregate<WorkOrderListQueryResultItem, ReferencedWorkOrderListItem>,
    ISelfRegisteredService<IAggregatePredicateBuilders<WorkOrderListQueryResultItem, ReferencedWorkOrderListItem>>
{
    public ReferencedWorkOrderPredicateBuilder()
    {
        
    }
}