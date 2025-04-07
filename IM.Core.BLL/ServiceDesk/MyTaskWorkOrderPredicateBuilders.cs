using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    internal class MyTaskWorkOrderPredicateBuilders : FilterBuildersAggregate<WorkOrder, MyTasksReportItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<WorkOrder, MyTasksReportItem>>
    {
        public MyTaskWorkOrderPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(MyTasksReportItem.Name), x => x.Name);
            AddPredicateBuilder(nameof(MyTasksReportItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.TypeFullName), x => x.Type.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.OwnerFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientFullName), x => x.InitiatorID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientSubdivisionFullName), x => x.IMObjID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientOrganizationName), x => x.Initiator.Subdivision.Organization.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateRegistered), x => x.UtcDateAssigned);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateClosed), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(MyTasksReportItem.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(MyTasksReportItem.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(MyTasksReportItem.Number), x => x.Number);
            AddPredicateBuilder(nameof(MyTasksReportItem.CategoryName), x => Issues.WorkOrder);
        }
    }
}
