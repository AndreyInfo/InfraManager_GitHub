using System;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс представляет одну строку результата выборки данных для отчета "Все массовые инциденты"
    /// </summary>
    public class AllMassIncidentsReportQueryResultItem
    {
        public int ID { get; init; }
        public Guid IMObjID { get; init; }
        public string ServiceName { get; init; }
        public short InformationChannelID { get; init; }
        public string Priority { get; init; }
        public int PriorityColor { get; init; }
        public int PrioritySequence { get; init; }
        public string Criticality { get; init; }
        public string Type { get; init; }
        public string Cause { get; init; }
        public int DocumentsQuantity { get; init; }
        public string ShortDescription { get; init; }
        public string FullDescription { get; init; }
        public string Solution { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public string EntityStateName { get; init; }
        public DateTime UtcCreatedAt { get; init; }
        public DateTime UtcLastModifiedAt { get; init; }
        public DateTime? UtcOpenedAt { get; init; }
        public DateTime? UtcRegisteredAt { get; init; }
        public DateTime? UtcCloseUntil { get; init; }
        public Guid InitiatorID { get; init; }
        public string CreatedByUserName { get; init; }
        public Guid OwnerID { get; init; }
        public string OwnedByUserName { get; init; }
        public string GroupName { get; init; }
        public string ServiceLevelAgreement { get; init; }
        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public string EntityStateID { get; init; }
    }
}
