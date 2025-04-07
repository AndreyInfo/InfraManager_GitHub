using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.FormDataValue;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class CallData : IServiceDeskEntityData
    {
        public Guid? PriorityID { get; init; }
        public Guid? InfluenceID { get; init; }
        public string Solution { get; init; }
        public Guid? RequestForServiceResultID { get; init; }
        public Guid? IncidentResultID { get; init; }
        public CallReceiptType? ReceiptType { get; init; }
        public Guid? InitiatorID { get; init; }
        public Guid? ClientID { get; init; }
        public NullablePropertyWrapper<Guid> OwnerID { get; init; }
        public NullablePropertyWrapper<Guid> ExecutorID { get; init; }
        public NullablePropertyWrapper<Guid> QueueID { get; init; }
        public NullablePropertyWrapper<Guid> AccomplisherID { get; init; }
        public Guid? CallTypeID { get; init; }
        public Guid? UrgencyID { get; init; }
        public string CallSummaryName { get; init; }
        public string Description { get; init; }
        public Guid? ServiceItemAttendanceID { get; init; }
        public Guid? KBArticleID { get; init; }
        public int? ObjectClassID { get; init; }
        public Guid? ObjectID { get; init; }
        public Guid? ServicePlaceID { get; init; }
        public int? ServicePlaceClassID { get; init; }
        public string UtcDatePromisedMilliseconds { get; init; }
        public string UtcDateRegistered { get; init; }
        public string UtcDateOpened { get; init; }
        public string UtcDateAccomplished { get; init; }
        public string UtcDateClosed { get; init; }
        public string EntityState { get; init; }
        public bool EntityStateIDIsNull { get; init; }
        public string EntityStateName { get; init; }
        public bool? IsSuspended { get; init; }
        public int? ManhoursNormInMinutes { get; init; }
        public string UserField1 { get; init; }
        public string UserField2 { get; init; }
        public string UserField3 { get; init; }
        public string UserField4 { get; init; }
        public string UserField5 { get; init; }
        public NullablePropertyWrapper<Guid> WorkflowSchemeID { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public byte? Grade { get; init; }
        public FormValuesData FormValuesData { get; init; }

        /// <summary>
        /// Признак необходимости расчета SLA
        /// </summary>
        public RefreshAgreement? RefreshAgreement { get; init; }

        /// <summary>
        /// Признак клиентской заявки
        /// </summary>
        public bool CreatedByClient { get; init; }

        public Guid? CalendarWorkScheduleID { get; init; }
        public int EscalationCount { get; init; }
        public byte? LineNumber { get; init; }
        public decimal? Price { get; init; }
        public string IncidentResultName { get;init; }
        public bool? OnWorkOrderExecutorControl { get; init; }

    }
}
