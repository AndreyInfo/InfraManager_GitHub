using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls
{
    public class CallDetailsModel : IUserFieldsModel
    {
        public Guid ID { get; init; }
        public ObjectClass ClassID => ObjectClass.Call;
        public string FullName => $"IM-CL-{Number}";
        public int Number { get; init; }
        public byte? Grade { get; set; }
        public string EntityStateID { get; init; }
        public string EntityStateName { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public string WorkflowSchemeID { get; init; }
        public byte ReceiptType { get; init; }
        public string ReceiptTypeName { get; set; }
        public string Description { get; init; }
        public string HTMLDescription { get; init; }
        public string Solution { get; init; }
        public string HTMLSolution { get; init; }
        public Guid? RFCResultID { get; init; }
        public string RFCResultName { get; set; }
        public Guid? IncidentResultID { get; init; }
        public string IncidentResultName { get; set; }
        public DateTime UtcDateCreated { get; init; }
        public string UtcDateRegistered { get; init; }
        public string UtcDateOpened { get; init; }
        public string UtcDatePromised { get; init; }
        public string UtcDateAccomplished { get; init; }
        public string UtcDateClosed { get; init; }
        public string UtcDateModified { get; init; }
        public string SLAName { get; init; }
        public string ServiceID { get; init; }
        public string ServiceCategoryName { get; init; }
        public string ServiceName { get; init; }
        public string ServiceItemID { get; init; }
        public string ServiceItemName { get; init; }
        public string ServiceAttendanceID { get; init; }
        public string ServiceAttendanceName { get; init; }
        public decimal? Price { get; init; }
        public Guid? ServicePlaceID { get; init; }
        public int? ServicePlaceClassID { get; init; }
        public string ServicePlaceName { get; init; }
        public string ServicePlaceNameShort { get; init; }
        public Guid CallTypeID { get; init; }
        public string CallType { get; init; }
        public bool IsRFCCallType { get; private set; }
        public bool IsIncidentResultCallType { get; private set; }
        public bool HaveNegotiationsWithCurrentUser { get; private set; }
        public string CallSummaryName { get; init; }
        public string InitiatorID { get; init; }
        public string ClientID { get; init; }
        public string OwnerID { get; init; }
        public string ExecutorID { get; init; }
        public string AccomplisherID { get; init; }
        public string UrgencyID { get; init; }
        public string UrgencyName { get; init; }
        public string InfluenceID { get; init; }
        public string InfluenceName { get; init; }
        public Guid PriorityID { get; init; }
        public string PriorityName { get; init; }
        public string PriorityColor { get; init; }
        public int ManhoursInMinutes { get; init; }
        public int ManhoursNormInMinutes { get; init; }
        public string QueueID { get; init; }
        public string QueueName { get; init; }
        public int DependencyObjectCount { get; init; }
        public int UnreadNoteCount { get; init; }
        public int NegotiationCount { get; init; }
        public int ProblemsRefCount { get; init; }
        public int WorkOrdersRefCount { get; init; }
        public int NoteCount { get; init; }
        public int MessageCount { get; init; }
        public int EscalationCount { get; init; }
        public bool SettingHidePlaceOfService { get; set; }
        public bool HaveUnvotedNegotiation { get; set; }
        public bool OnWorkOrderExecutorControl { get; init; }
        public Guid? CalendarWorkScheduleID { get; init; }
        public string TimeZoneID { get; init; }
        public byte? LineNumber { get; init; }
        public string ManhoursString { get; init; }
        public string ManhoursNormString { get; init; }
        public FormValuesDetailsModel FormValues { get; init; }
    }
}
