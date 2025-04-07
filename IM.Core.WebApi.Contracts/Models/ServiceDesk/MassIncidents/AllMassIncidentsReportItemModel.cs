using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents
{
    public class AllMassIncidentsReportItemModel
    {
        public string Uri { get; init; }
        public ObjectClass ClassID => ObjectClass.MassIncident;
        public int ID { get; init; }
        public int Number { get; init; }
        public Guid IMObjID { get; init; }
        public string ServiceName { get; init; }
        public string InformationChannel { get; init; }
        public string Priority { get; init; }
        public string PriorityColor { get; init; }
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
        public string UtcCreatedAt { get; init; }
        public string UtcLastModifiedAt { get; init; }
        public string UtcOpenedAt { get; init; }
        public string UtcRegisteredAt { get; init; }
        public string UtcCloseUntil { get; init; }
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
