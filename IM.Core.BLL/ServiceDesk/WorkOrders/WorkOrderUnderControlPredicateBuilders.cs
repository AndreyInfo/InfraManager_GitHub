using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderUnderControlPredicateBuilders : FilterBuildersAggregate<WorkOrder, ObjectUnderControl>,
         ISelfRegisteredService<IAggregatePredicateBuilders<WorkOrder, ObjectUnderControl>>
    {
        public WorkOrderUnderControlPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(ObjectUnderControl.CategoryName), x => Issues.WorkOrder);
            AddPredicateBuilder(nameof(ObjectUnderControl.Name), x => x.Name);
            AddPredicateBuilder(nameof(ObjectUnderControl.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(ObjectUnderControl.TypeFullName), x => x.Type.ID);
            AddPredicateBuilder(nameof(ObjectUnderControl.OwnerFullName), x => x.AssigneeID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientFullName), x => x.InitiatorID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientSubdivisionFullName), x => x.IMObjID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientOrganizationName), x => x.Initiator.Subdivision.Organization.ID);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateRegistered), x => x.UtcDateAssigned);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateClosed), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(ObjectUnderControl.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(ObjectUnderControl.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(ObjectUnderControl.Number), x => x.Number);
        }
    }
}
