using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MyTaskMassIncidentPredicateBuilders : FilterBuildersAggregate<MassIncident, MyTasksReportItem>,
        ISelfRegisteredService<IAggregatePredicateBuilders<MassIncident, MyTasksReportItem>>
    {
        public MyTaskMassIncidentPredicateBuilders() 
        {
            AddPredicateBuilder(nameof(MyTasksReportItem.Name), x => x.Name);
            AddPredicateBuilder(nameof(MyTasksReportItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.TypeFullName), x => x.Type.IMObjID);
            AddPredicateBuilder(nameof(MyTasksReportItem.OwnerFullName), x => x.OwnedBy.IMObjID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ExecutorFullName), x => x.ExecutedByUser.IMObjID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientFullName), x => x.CreatedBy.IMObjID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientSubdivisionFullName), x => x.CreatedBy.SubdivisionID);
            AddPredicateBuilder(nameof(MyTasksReportItem.ClientOrganizationName), x => x.CreatedBy.Subdivision.Organization.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateRegistered), x => x.UtcRegisteredAt);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDatePromised), x => x.UtcCloseUntil);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(MyTasksReportItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(MyTasksReportItem.QueueName), x => x.ExecutedByGroupID);
            AddPredicateBuilder(nameof(MyTasksReportItem.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(MyTasksReportItem.Number), x => x.ID);
            AddPredicateBuilder(nameof(MyTasksReportItem.CategoryName), x => Issues.MassIncident);
        }
    }
}
