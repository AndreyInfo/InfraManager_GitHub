using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationWorkOrderPredicateBuilders : FilterBuildersAggregate<WorkOrder, NegotiationListItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<WorkOrder, NegotiationListItem>>
    {
        public NegotiationWorkOrderPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(NegotiationListItem.CategoryName), x => Issues.WorkOrder);
            AddPredicateBuilder(nameof(NegotiationListItem.Name), x => x.Name);
            AddPredicateBuilder(nameof(NegotiationListItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(NegotiationListItem.TypeFullName), x => x.Type.ID);
            AddPredicateBuilder(nameof(NegotiationListItem.OwnerFullName), x => x.AssigneeID);
            AddPredicateBuilder(nameof(NegotiationListItem.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientFullName), x => x.InitiatorID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientSubdivisionFullName), x => x.IMObjID);
            AddPredicateBuilder(nameof(NegotiationListItem.ClientOrganizationName), x => x.Initiator.Subdivision.Organization.ID);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateRegistered), x => x.UtcDateAssigned);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateClosed), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(NegotiationListItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(NegotiationListItem.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(NegotiationListItem.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(NegotiationListItem.Number), x => x.Number);
        }
    }
}
