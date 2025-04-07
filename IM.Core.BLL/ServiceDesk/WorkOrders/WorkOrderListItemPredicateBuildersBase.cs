using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderListItemPredicateBuildersBase<TListQueryResultItem, TListItem> :
        FilterBuildersAggregate<TListQueryResultItem, TListItem> where TListQueryResultItem : WorkOrderListQueryResultItemBase where TListItem : WorkOrderListItemBase
    {
        public WorkOrderListItemPredicateBuildersBase() : base()
        {
            AddPredicateBuilder(x => x.BudgetString);
            AddPredicateBuilder(x => x.BudgetUsageCauseString);
            AddPredicateBuilder(x => x.EntityStateName);
            AddPredicateBuilder(x => x.UnreadMessageCount);
            AddPredicateBuilder(x => x.DocumentCount);
            AddPredicateBuilder(x => x.ReferencedObjectNumberString);
        }
    }
}