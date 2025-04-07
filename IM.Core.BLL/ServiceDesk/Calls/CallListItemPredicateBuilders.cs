using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallListItemPredicateBuilders :
        FilterBuildersAggregate<AllCallsQueryResultItem, CallListItem>,
        ISelfRegisteredService<IAggregatePredicateBuilders<AllCallsQueryResultItem, CallListItem>>
    {
        public CallListItemPredicateBuilders() : base()
        {
            AddPredicateBuilder(x => x.BudgetString);
            AddPredicateBuilder(x => x.BudgetUsageCauseString);
            AddPredicateBuilder(x => x.UnreadMessageCount);
            AddPredicateBuilder(x => x.DocumentCount);
            AddPredicateBuilder(x => x.ProblemCount);
            AddPredicateBuilder(x => x.WorkOrderCount);
        }
    }
}
