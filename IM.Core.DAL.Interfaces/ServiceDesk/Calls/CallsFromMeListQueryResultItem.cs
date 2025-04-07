using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    public class CallsFromMeListQueryResultItem : ServiceDeskQueryResultItemBase
    {
        public CallReceiptType ReceiptType { get; init; }

        public string TypeFullName { get; init; }

        public string Summary { get; init; }
        public string Description { get; init; }
        public string Solution { get; init; }
        public byte? Grade { get; init; }
        public string ServiceName { get; init; }

        public string SLAName { get; init; }
        public decimal? Price { get; init; }
        public int UrgencySequence { get; init; } 
        public string UrgencyName { get; init; }

        public DateTime? UtcDateRegistered { get; init; }
        public DateTime UtcDatePromised { get; init; }
        public DateTime? UtcDateAccomplished { get; init; }
        public DateTime? UtcDateClosed { get; init; }
        public DateTime UtcDateModified { get; init; }
        public DateTime UtcDateCreated { get; init; }

        public string ClientFullName { get; init; }
        public string ClientSubdivisionFullName { get; init; }
        public string ClientOrganizationName { get; init; }

        public string InitiatorFullName { get; init; }
        public string ServiceItemOrAttendance { get; init; }

        public int DocumentCount { get; init; }
        public int UnreadMessageCount { get; init; }
        public bool InControl { get; init; }
        public bool CanBePicked { get; init; }
        public int NoteCount { get; init; }
        public int MessageCount { get; init; }

        public Guid? OwnerID { get; init; }
        public string OwnerFullName { get; init; }
        public Guid? ClientID { get; init; }
        public Guid? ExecutorID { get; init; }
        public int PriorityColor { get; init; }
    }
}
