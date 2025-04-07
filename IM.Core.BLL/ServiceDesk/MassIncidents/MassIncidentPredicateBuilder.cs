using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentPredicateBuilder :
        FilterBuildersAggregate<MassIncident, AllMassIncidentsReportItem>,
        ISelfRegisteredService<IAggregatePredicateBuilders<MassIncident, AllMassIncidentsReportItem>>
    {
        public MassIncidentPredicateBuilder()
        {
            AddPredicateBuilder(reportItem => reportItem.Solution, massIncident => massIncident.Solution.Plain);
            AddPredicateBuilder(reportItem => reportItem.Cause, massIncident => massIncident.CauseID);
            AddPredicateBuilder(reportItem => reportItem.CreatedByUserName, massIncident => massIncident.CreatedBy.IMObjID);
            AddPredicateBuilder(reportItem => reportItem.Criticality, massIncident => massIncident.CriticalityID);
            AddPredicateBuilder(reportItem => reportItem.FullDescription, massIncident => massIncident.Description.Plain);
            AddPredicateBuilder(reportItem => reportItem.GroupName, massIncident => massIncident.ExecutedByGroupID);
            AddPredicateBuilder(reportItem => reportItem.InformationChannel, massIncident => massIncident.InformationChannelID);
            AddPredicateBuilder(reportItem => reportItem.OwnedByUserName, massIncident => massIncident.OwnedBy.IMObjID);
            AddPredicateBuilder(reportItem => reportItem.Priority, massIncident => massIncident.PriorityID);
            AddPredicateBuilder(reportItem => reportItem.OperationalLevelAgreement, massIncident => massIncident.OperationalLevelAgreementID);
            AddPredicateBuilder(reportItem => reportItem.ServiceName, massIncident => massIncident.ServiceID);
            AddPredicateBuilder(reportItem => reportItem.EntityStateName, massIncident => massIncident.EntityStateName);
            AddPredicateBuilder(reportItem => reportItem.Type, massIncident => massIncident.TypeID);
            AddPredicateBuilder(reportItem => reportItem.UtcCloseUntil, massIncident => massIncident.UtcCloseUntil);
            AddPredicateBuilder(reportItem => reportItem.UtcCreatedAt, massIncident => massIncident.UtcCreatedAt);
            AddPredicateBuilder(reportItem => reportItem.UtcLastModifiedAt, massIncident => massIncident.UtcDateModified);
            AddPredicateBuilder(reportItem => reportItem.UtcOpenedAt, massIncident => massIncident.UtcOpenedAt);
            AddPredicateBuilder(reportItem => reportItem.UtcRegisteredAt, massIncident => massIncident.UtcRegisteredAt);
            AddPredicateBuilder(reportItem => reportItem.Name, massIncident => massIncident.Name);
            AddPredicateBuilder(reportItem => reportItem.Number, massIncident => massIncident.ID);
        }
    }
}
