using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallUnderControlPredicateBuilders : FilterBuildersAggregate<Call, ObjectUnderControl>,
         ISelfRegisteredService<IAggregatePredicateBuilders<Call, ObjectUnderControl>>
    {
        public CallUnderControlPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(ObjectUnderControl.CategoryName), x => Issues.Call);
            AddPredicateBuilder(nameof(ObjectUnderControl.Name), x => x.CallSummaryName);
            AddPredicateBuilder(nameof(ObjectUnderControl.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(ObjectUnderControl.TypeFullName), x => x.CallType.ID);
            AddPredicateBuilder(nameof(ObjectUnderControl.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientFullName), x => x.ClientID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientSubdivisionFullName), x => x.ClientSubdivisionID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientOrganizationName), x => x.ClientSubdivision.Organization.ID);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateRegistered), x => x.UtcDateRegistered);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(ObjectUnderControl.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(ObjectUnderControl.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(ObjectUnderControl.Number), x => x.Number);
        }
    }
}
