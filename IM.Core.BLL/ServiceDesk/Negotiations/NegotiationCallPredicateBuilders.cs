using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationCallPredicateBuilders : FilterBuildersAggregate<Call, NegotiationListItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<Call, NegotiationListItem>>
    {
        public NegotiationCallPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(NegotiationListItem.CategoryName), x => Issues.Call);
            AddPredicateBuilder(nameof(NegotiationListItem.Name), x => x.CallSummaryName);
            AddPredicateBuilder(nameof(NegotiationListItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(NegotiationListItem.TypeFullName), x => x.CallType.ID);
            AddPredicateBuilder(nameof(NegotiationListItem.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(NegotiationListItem.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientFullName), x => x.ClientID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientSubdivisionFullName), x => x.ClientSubdivisionID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientOrganizationName), x => x.ClientSubdivision.Organization.ID);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateRegistered), x => x.UtcDateRegistered);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(NegotiationListItem.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(NegotiationListItem.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(NegotiationListItem.Number), x => x.Number);
        }
    }
}
