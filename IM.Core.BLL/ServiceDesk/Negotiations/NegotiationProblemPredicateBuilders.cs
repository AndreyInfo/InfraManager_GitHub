using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationProblemPredicateBuilders : FilterBuildersAggregate<Problem, NegotiationListItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<Problem, NegotiationListItem>>
    {
        public NegotiationProblemPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(NegotiationListItem.CategoryName), x => Issues.Problem);
            AddPredicateBuilder(nameof(NegotiationListItem.Name), x => x.Summary);
            AddPredicateBuilder(nameof(NegotiationListItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(NegotiationListItem.TypeFullName), x => x.Type.ID);
            AddPredicateBuilder(nameof(NegotiationListItem.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(NegotiationListItem.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(NegotiationListItem.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientFullName), x => x.InitiatorID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientSubdivisionFullName), x => x.Initiator.SubdivisionID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientOrganizationName), x => x.Initiator.Subdivision.OrganizationID);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateRegistered), x => x.UtcDateDetected);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateAccomplished), x => x.UtcDateSolved);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(NegotiationListItem.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(NegotiationListItem.Number), x => x.Number);
        }
    }
}
