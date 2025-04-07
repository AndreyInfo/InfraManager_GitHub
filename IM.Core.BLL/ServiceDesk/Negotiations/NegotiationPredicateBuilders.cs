using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationPredicateBuilders : FilterBuildersAggregate<Negotiation, NegotiationListItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<Negotiation, NegotiationListItem>>
    {
        public NegotiationPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(NegotiationListItem.NegotiationName), x => x.Name);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcNegotiationDateVoteStart), x => x.UtcDateVoteStart);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcNegotiationDateVoteEnd), x => x.UtcDateVoteEnd);
            AddPredicateBuilder(nameof(NegotiationListItem.NegotiationStatusString), x => x.Status); 
            AddPredicateBuilder(nameof(NegotiationListItem.NegotiationModeString), x => x.Mode);
        }
    }
}

