using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class ProblemUnderControlPredicateBuilders : FilterBuildersAggregate<Problem, ObjectUnderControl>,
         ISelfRegisteredService<IAggregatePredicateBuilders<Problem, ObjectUnderControl>>
    {
        public ProblemUnderControlPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(ObjectUnderControl.CategoryName), x => Issues.Problem);
            AddPredicateBuilder(nameof(ObjectUnderControl.Name), x => x.Summary);
            AddPredicateBuilder(nameof(ObjectUnderControl.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(ObjectUnderControl.TypeFullName), x => x.Type.ID);
            AddPredicateBuilder(nameof(ObjectUnderControl.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(ObjectUnderControl.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientFullName), x => x.InitiatorID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientSubdivisionFullName), x => x.Initiator.SubdivisionID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientOrganizationName), x => x.Initiator.Subdivision.OrganizationID);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateRegistered), x => x.UtcDateDetected);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateAccomplished), x => x.UtcDateSolved);
            AddPredicateBuilder(nameof(ObjectUnderControl.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(ObjectUnderControl.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(ObjectUnderControl.Number), x => x.Number);
        }
    }
}
