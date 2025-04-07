using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls
{
    public class CallsFromMeListItemModel
    {
        public string Uri { get; set; }
        public Guid IMObjID => ID;
        public Guid ID { get; set; }
        public int ClassID { get; set; }
        public Guid? OwnerID { get; set; }
        public int MessageCount { get; set; }
        public int NoteCount { get; set; }
        public bool InControl { get; set; }
        public bool CanBePicked { get; set; }
        public bool HasState { get; set; }
        public Guid TypeID { get; set; }
        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeIdentifier { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        public int UnreadMessageCount { get; set; }
        public int Number { get; set; }
        public string PriorityColor { get; set; }
        public string ClientFullName { get; set; }
        public string ClientSubdivisionFullName { get; set; }
        public string ClientOrganizationName { get; set; }
        public string InitiatorFullName { get; set; }
        public string ReceiptTypeString { get; set; }
        public string TypeFullName { get; set; }
        public int DocumentCount { get; set; }
        public string Summary { get; set; }
        public string ServiceName { get; set; }
        public string ServiceItemOrAttendance { get; set; }
        public string EntityStateID { get; init; }
        public string EntityStateName { get; set; }
        public string UrgencyName { get; set; }
        public string Description { get; set; }
        public byte? Grade { get; set; }
        public string Solution { get; set; }
        public DateTime? UtcDateRegistered { get; set; }
        public DateTime UtcDatePromised { get; set; }
        public DateTime? UtcDateAccomplished { get; set; }
        public DateTime? UtcDateClosed { get; set; }
        public DateTime UtcDateModified { get; set; }
        public string SLAName { get; set; }
        public decimal? Price { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }
        public string UserField4 { get; set; }
        public string UserField5 { get; set; }
        public DateTime UtcDateCreated { get; set; }
        public string OwnerFullName { get; set; }
        public string ImageSource { get; set; }
    }
}
