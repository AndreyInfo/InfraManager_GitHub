using InfraManager.BLL.ServiceDesk.Negotiations;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentNegotiationPredicateBuilder :
        FilterBuildersAggregate<MassIncident, NegotiationListItem>,
        ISelfRegisteredService<IAggregatePredicateBuilders<MassIncident, NegotiationListItem>>
    {
        public MassIncidentNegotiationPredicateBuilder()
        {
            AddPredicateBuilder(nameof(NegotiationListItem.CategoryName), x => Issues.MassIncident);
            AddPredicateBuilder(nameof(NegotiationListItem.Name), x => x.Name);
            AddPredicateBuilder(nameof(NegotiationListItem.PriorityName), x => x.PriorityID);
            AddPredicateBuilder(nameof(NegotiationListItem.TypeFullName), x => x.TypeID);
            AddPredicateBuilder(nameof(NegotiationListItem.OwnerFullName), x => x.OwnedBy.IMObjID);
            AddPredicateBuilder(nameof(NegotiationListItem.ExecutorFullName), x => x.ExecutedByUser.IMObjID);
            AddPredicateBuilder(nameof(NegotiationListItem.QueueName), x => x.ExecutedByGroupID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientFullName), x => x.CreatedBy.IMObjID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientSubdivisionFullName), x => x.CreatedBy.SubdivisionID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientOrganizationName), x => x.CreatedBy.OrganizationId);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateRegistered), x => x.UtcRegisteredAt);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDatePromised), x => (DateTime?)null);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(NegotiationListItem.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(NegotiationListItem.Number), x => x.ID);
        }
    }
}
