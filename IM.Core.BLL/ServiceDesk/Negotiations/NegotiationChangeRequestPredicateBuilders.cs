using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationRFCPredicateBuilders : FilterBuildersAggregate<ChangeRequest, NegotiationListItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<ChangeRequest, NegotiationListItem>>
    {
        public NegotiationRFCPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(NegotiationListItem.CategoryName), x => Issues.ChangeRequest);
            AddPredicateBuilder(nameof(NegotiationListItem.Name), x => x.Summary);
            AddPredicateBuilder(nameof(NegotiationListItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(NegotiationListItem.TypeFullName), x => x.Type.ID);
            AddPredicateBuilder(nameof(NegotiationListItem.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(NegotiationListItem.ExecutorFullName), x => Guid.Empty);
            AddPredicateBuilder(nameof(NegotiationListItem.QueueName), x => Guid.Empty);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientFullName), x => Guid.Empty);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientSubdivisionFullName), x => Guid.Empty);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientOrganizationName), x => Guid.Empty);
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
