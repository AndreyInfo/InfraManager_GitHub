using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestUnderControlPredicateBuilders : FilterBuildersAggregate<ChangeRequest, ObjectUnderControl>,
         ISelfRegisteredService<IAggregatePredicateBuilders<ChangeRequest, ObjectUnderControl>>
    {
        public ChangeRequestUnderControlPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(ObjectUnderControl.CategoryName), x => Issues.ChangeRequest);
            AddPredicateBuilder(nameof(ObjectUnderControl.Name), x => x.Summary);
            AddPredicateBuilder(nameof(ObjectUnderControl.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(ObjectUnderControl.TypeFullName), x => x.Type.ID);
            AddPredicateBuilder(nameof(ObjectUnderControl.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(ObjectUnderControl.ExecutorFullName), x => Guid.Empty);
            AddPredicateBuilder(nameof(ObjectUnderControl.QueueName), x => Guid.Empty);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientFullName), x => Guid.Empty);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientSubdivisionFullName), x => Guid.Empty);
            AddPredicateBuilder(nameof(ObjectUnderControl.ClientOrganizationName), x => Guid.Empty);
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
