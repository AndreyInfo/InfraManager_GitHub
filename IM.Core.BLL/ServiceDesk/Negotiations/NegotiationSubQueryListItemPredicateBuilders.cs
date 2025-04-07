using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationSubQueryListItemPredicateBuilders : FilterBuildersAggregate<NegotiationListSubQueryResultItem, NegotiationListItem>, ISelfRegisteredService<IAggregatePredicateBuilders<NegotiationListSubQueryResultItem, NegotiationListItem>>
    {
        public NegotiationSubQueryListItemPredicateBuilders() : base()
        {
        }
    }
}
