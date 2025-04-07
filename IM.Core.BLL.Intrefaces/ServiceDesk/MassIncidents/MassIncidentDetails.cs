using System;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс описывает контракт выходных данных массового инцидента
    /// </summary>
    public class MassIncidentDetails : ISDEntityWithPriorityColorInt
    {
        public int ID { get; init; }
        public Guid IMObjID { get; init; }
        public ObjectClass ClassID => ObjectClass.MassIncident;
        public string Name { get; init; }
        public string Description { get; init; }
        public string Solution { get; init; }
        public short InformationChannelID { get; init; }
        public int TypeID { get; init; }
        public Guid? CreatedByUserID { get; init; }
        public Guid? OwnedByUserID { get; init; }
        public Guid? ExecutedByUserID { get; init; }
        public Guid? ExecutedByGroupID { get; init; }
        public Guid ServiceID { get; init; }
        public Guid? ServiceLevelAgreementID { get; init; }
        public Guid PriorityID { get; init; }
        public int PriorityColor { get; init; }
        public Guid? CriticalityID { get; init; }
        public int? CauseID { get; init; }
        public string Cause { get; init; }
        public DateTime? UtcCloseUntil { get; init; }
        public DateTime UtcCreatedAt { get; init; }
        public DateTime UtcDateModified { get; init; }
        public DateTime? UtcOpenedAt { get; init; }
        public DateTime? UtcRegisteredAt { get; init; }
        public DateTime? UtcDateAccomplished { get; init; }
        public DateTime? UtcDateClosed { get; init; }
        public string EntityStateID { get; init; }
        public string EntityStateName { get; init; }
        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public int? TechnicalFailureCategoryID { get; init; }

        /// <summary>
        /// Возвращает или задает данные формы доп. параметров.
        /// </summary>
        public FormValuesDetailsModel FormValues { get; init; }
        public int WorkOrderCount { get; set; }
        public int ProblemCount { get; init; }
        public int CallCount { get; init; }
        public int RFCCount { get; init; }

    }
}
