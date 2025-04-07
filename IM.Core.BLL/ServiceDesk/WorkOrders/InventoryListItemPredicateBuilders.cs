using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class InventoryListItemPredicateBuilders : WorkOrderListItemPredicateBuildersBase<InventoryListQueryResultItem, InventoryListItem>,
        ISelfRegisteredService<IAggregatePredicateBuilders<InventoryListQueryResultItem, InventoryListItem>>
    { }
}