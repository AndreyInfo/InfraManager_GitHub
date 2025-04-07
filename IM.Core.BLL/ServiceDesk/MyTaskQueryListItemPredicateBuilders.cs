using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    internal class MyTaskQueryListItemPredicateBuilders : 
        FilterBuildersAggregate<MyTasksListQueryResultItem, MyTasksReportItem>, 
        ISelfRegisteredService<IAggregatePredicateBuilders<MyTasksListQueryResultItem, MyTasksReportItem>>
    {
        public MyTaskQueryListItemPredicateBuilders() : base()
        {
            AddPredicateBuilder(x => x.UnreadMessageCount);
            AddPredicateBuilder(x => x.DocumentCount);
        }
    }
}
