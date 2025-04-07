using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk
{
    internal class MyTaskProblemPredicateBuilders : FilterBuildersAggregate<Problem, MyTasksReportItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<Problem, MyTasksReportItem>>
    {
        public MyTaskProblemPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(MyTasksReportItem.Name), x => x.Summary);
            AddPredicateBuilder(nameof(MyTasksReportItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.TypeFullName), x => x.Type.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(MyTasksReportItem.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientFullName), x => Guid.Empty);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientSubdivisionFullName), x => Guid.Empty);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientOrganizationName), x => Guid.Empty);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateRegistered), x => x.UtcDateDetected);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateAccomplished), x => x.UtcDateSolved);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(MyTasksReportItem.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(MyTasksReportItem.Number), x => x.Number);
            AddPredicateBuilder(nameof(MyTasksReportItem.CategoryName), x => Issues.Problem);
        }
    }
}
