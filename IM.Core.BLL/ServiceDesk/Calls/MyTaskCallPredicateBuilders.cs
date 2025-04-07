using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class MyTaskCallPredicateBuilders : FilterBuildersAggregate<Call, MyTasksReportItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<Call, MyTasksReportItem>>
    {
        public MyTaskCallPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(MyTasksReportItem.Name), x => x.CallSummaryName);
            AddPredicateBuilder(nameof(MyTasksReportItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.TypeFullName), x => x.CallType.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientFullName), x => x.ClientID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientSubdivisionFullName), x => x.ClientSubdivisionID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientOrganizationName), x => x.ClientSubdivision.Organization.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateRegistered), x => x.UtcDateRegistered);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(MyTasksReportItem.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(MyTasksReportItem.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(MyTasksReportItem.Number), x => x.Number);
            AddPredicateBuilder(nameof(MyTasksReportItem.CategoryName), x => Issues.Call);
        }
    }
}
