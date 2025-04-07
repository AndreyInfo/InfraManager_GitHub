using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class InventoryPredicateBuilders : WorkOrderPredicateBuildersBase<InventoryListItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<WorkOrder, InventoryListItem>>
    { }
}
