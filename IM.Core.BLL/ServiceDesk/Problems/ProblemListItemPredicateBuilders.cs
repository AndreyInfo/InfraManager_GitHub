using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.Problems;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class ProblemListItemPredicateBuilders :
        FilterBuildersAggregate<ProblemListQueryResultItem, ProblemListItem>,
        ISelfRegisteredService<IAggregatePredicateBuilders<ProblemListQueryResultItem, ProblemListItem>>
    {
        public ProblemListItemPredicateBuilders() : base()
        {
            AddPredicateBuilder(x => x.BudgetString);
            AddPredicateBuilder(x => x.BudgetUsageCauseString);
            AddPredicateBuilder(x => x.EntityStateName);
            AddPredicateBuilder(x => x.UnreadMessageCount);
            AddPredicateBuilder(x => x.DocumentCount);
            AddPredicateBuilder(x => x.CallCount);
            AddPredicateBuilder(x => x.WorkOrderCount);
        }
    }
}
