using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents
{
    public class MassIncidentDetailsModel
    {
        public long ID { get; init; }
        public Guid IMObjID { get; init; }
        public string Uri { get; init; }
        public ObjectClass ClassID { get; init; }
        /// <summary>
        /// Возвращает название МИ (он же краткое описание, он же саммари)
        /// </summary>
        public string Name { get; init; }
        public string Description { get; init; }
        public string Solution { get; init; }
        public short InformationChannelID { get; init; }
        public string InformationChannelUri { get; init; }
        public int TypeID { get; init; }
        public string TypeUri { get; init; }
        public Guid? CreatedByUserID { get; init; }
        public string CreatedByUserUri { get; init; }
        public Guid? OwnedByUserID { get; init; }
        public string OwnedByUserUri { get; init; }
        public Guid? ExecutedByUserID { get; init; }
        public string ExecutedByUserUri { get; init; }

        public Guid? ExecutedByGroupID { get; init; }
        public string ExecutedByGroupUri { get; init; }
        public Guid ServiceID { get; init; }
        public string ServiceUri { get; init; }
        public Guid SlaID { get; init; }
        public Guid PriorityID { get; init; }
        public string PriorityUri { get; init; }
        public string PriorityColor { get; init; }
        public Guid? CriticalityID { get; init; }
        public string CriticalityUri { get; init; }
        public int? CauseID { get; init; }
        public string CauseUri { get; init; }
        public string UtcCloseUntil { get; init; }
        public string UtcCreatedAt { get; init; }
        public string UtcRegisteredAt { get; init; }
        public string UtcDateModified { get; init; }
        public string UtcOpenedAt { get; init; }
        public string UtcDateClosed { get; init; }
        public string EntityStateID { get; init; }
        public string EntityStateName { get; init; }
        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public int? TechnicalFailureCategoryID { get; init; }
        public string TechnicalFailureCategoryUri { get; init; }
        public string Cause { get; init; }
        public string UtcDateAccomplished { get; init; }

        /// <summary>
        /// Возвращает или задает данные формы доп. параметров.
        /// </summary>
        public FormValuesDetailsModel FormValues { get; init; }
        public int WorkOrderCount { get; init; }
        public int ProblemCount { get; init; }
        public int CallCount { get; init; }
        public int RFCCount { get; init; }
    }
}
