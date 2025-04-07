using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.Calls;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallFromMeListItemPredicateBuilders :
        FilterBuildersAggregate<CallsFromMeListQueryResultItem, CallFromMeListItem>,
        ISelfRegisteredService<IAggregatePredicateBuilders<CallsFromMeListQueryResultItem, CallFromMeListItem>>
    {
        public CallFromMeListItemPredicateBuilders() : base()
        {
            AddPredicateBuilder(x => x.UnreadMessageCount);
            AddPredicateBuilder(x => x.DocumentCount);
        }
    }
}
