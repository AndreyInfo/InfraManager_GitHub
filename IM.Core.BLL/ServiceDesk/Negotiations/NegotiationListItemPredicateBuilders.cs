using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationListItemPredicateBuilders : FilterBuildersAggregate<NegotiationListQueryResultItem, NegotiationListItem>, ISelfRegisteredService<IAggregatePredicateBuilders<NegotiationListQueryResultItem, NegotiationListItem>>
    {
        public NegotiationListItemPredicateBuilders() : base()
        {
            AddPredicateBuilder(x => x.UnreadMessageCount);
            AddPredicateBuilder(x => x.DocumentCount);
        }
    }
}
