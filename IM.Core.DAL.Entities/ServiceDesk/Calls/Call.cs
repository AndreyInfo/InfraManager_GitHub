using Inframanager;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk.Calls;
using System;
using System.Collections.Generic;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.ServiceDesk.Manhours;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System.Linq.Expressions;
using InfraManager.Linq;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk
{
    [ObjectClassMapping(ObjectClass.Call)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Call_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Call_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Call_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)] // не трогать
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)] // не трогать
    public class Call : 
        IServiceDeskEntity, 
        IMarkableForDelete,
        Negotiations.ICreateNegotiation,
        IHaveManhours,
        ITimeZoneObject,
        IHaveUserFields,
        IHaveBudget,
        ICreateWorkOrderReference,
        IHaveFormValues, 
        IHaveUtcModifiedDate,
        IWorkOrderExecutorControl
    {
        #region .ctor

        protected Call()
        {
        }

        public Call(CallReceiptType receiptType)
        {
            IMObjID = Guid.NewGuid();            
            UtcDateCreated = DateTime.UtcNow;
            UtcDateRegistered = null;
            UtcDateModified = UtcDateCreated;
            ReceiptType = receiptType;
            UserField1 = string.Empty;
            UserField2 = string.Empty;
            UserField3 = string.Empty;
            UserField4 = string.Empty;
            UserField5 = string.Empty;
            Description = string.Empty;
            Solution = string.Empty;
            HTMLDescription = string.Empty;
            HTMLSolution = string.Empty;
            Aggregate = new CallAggregate(IMObjID);
        }

        #endregion

        #region Properties

        public Guid IMObjID { get; private set; }
        public int Number { get; private set; }
        public CallReceiptType ReceiptType { get; set; }
        public DateTime UtcDateModified { get; set; }
        public DateTime UtcDateCreated { get; private set; }
        public DateTime? UtcDateRegistered { get; set; }
        public DateTime? UtcDateOpened { get; set; }
        public DateTime UtcDatePromised { get; set; }
        public DateTime? UtcDateAccomplished { get; set; }
        public DateTime? UtcDateClosed { get; set; }
        public int EscalationCount { get; set; }
        public byte? Grade { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }
        public string UserField4 { get; set; }
        public string UserField5 { get; set; }
        public string SLAName { get; set; }
        public decimal? Price { get; set; }
        public string CallSummaryName { get; set; }
        public string HTMLDescription { get; set; }
        public string HTMLSolution { get; set; }
        public byte? LineNumber { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }
        public int ManhoursNormInMinutes { get; set; }
        public bool OnWorkOrderExecutorControl { get; set; }        
        public bool IsActive { get; }
        public bool IsFinished { get; }

        public static Expression<Func<Call, bool>> IsOverdue =>
            call => call.UtcDatePromised < DateTime.UtcNow;

        #endregion

        #region Call Type
        
        public Guid CallTypeID { get; set; }
        public virtual CallType CallType { get; }

        #endregion

        #region Employees

        public Guid? InitiatorID { get; set; }
        public Guid? OwnerID { get; set; }
        public Guid? ExecutorID { get; set; }
        public Guid? QueueID { get; private set; }
        public Guid? AccomplisherID { get; set; }
        public Guid InitiatorDefaultID { get; }
        public Guid ExecutorDefaultID { get; }
        public Guid OwnerDefaultID { get; }
        public Guid AccomplisherDefaultID { get; }
        public virtual User Initiator { get; init; }
        public virtual User Executor { get; init; }
        public virtual User Owner { get; init; }
        public virtual User Accomplisher { get; init; }
        public virtual Group Queue { get; private set; }

        public void SetGroup(Group newGroup)
        {
            QueueID = newGroup?.IMObjID;
            Queue = newGroup;
            Aggregate.QueueName = newGroup?.Name;
        }

        public static Expression<Func<Call, string>> OwnerFullName =>
            User.FullNameExpression.Substitute<Call, User, string>(call => call.Owner);

        public static Expression<Func<Call, string>> ExecutorFullName =>
            User.FullNameExpression.Substitute<Call, User, string>(call => call.Executor);

        public static Expression<Func<Call, bool>> CanBePicked(Guid userID) =>
            call => call.QueueID != null && call.Queue.QueueUsers.Any(u => u.UserID == userID) && call.ExecutorID == null;

        #endregion

        #region Client

        public Guid ClientID { get; set; }
        public virtual User Client { get; }
        public Guid? ClientSubdivisionID { get; set; }
        public Guid ClientSubdivisionDefaultID { get; }        
        public virtual Subdivision ClientSubdivision { get; }
        public static Expression<Func<Call, string>> ClientFullName =>
            User.FullNameExpression.Substitute<Call, User, string>(call => call.Client);

        public static Expression<Func<Call, string>> ClientSubdivisionFullName =>
            Subdivision.SubdivisionFullName.Substitute<Call, Guid?, string>(call => call.ClientSubdivisionID);

        #endregion

        #region Urgency

        public Guid? UrgencyID { get; set; }
        public Guid UrgencyDefaultID { get; }
        public virtual Urgency Urgency { get; }

        #endregion

        #region FormValues

        public long? FormValuesID { get; set; }
        public Guid? FormID { get; set; }

        public virtual FormValues FormValues { get; set; }
        #endregion

        #region Influence

        public Guid? InfluenceID { get; set; }
        public Guid InfluenceDefaultID { get; }
        public virtual Influence Influence { get; }

        #endregion

        #region Incident result

        public Guid? IncidentResultID { get; set; }
        public virtual IncidentResult IncidentResult { get; }

        #endregion

        #region RFS Result

        public Guid? RequestForServiceResultID { get; set; }
        public virtual RequestForServiceResult RequestForServiceResult { get; }

        #endregion

        #region Workflow

        public string EntityStateID { get; set; }
        public string EntityStateName { get; set; }
        public Guid? WorkflowSchemeID { get; set; }
        public string WorkflowSchemeIdentifier { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        public virtual WorkFlowScheme WorkflowScheme { get; }
        public string TargetEntityStateID { get; set; }

        #endregion

        #region Service Place

        public Guid? ServicePlaceID { get; set; }
        public ObjectClass? ServicePlaceClassID { get; set; }

        #endregion

        #region Service

        public Guid CallServiceID { get; private set; }
        public virtual CallService CallService { get; set; }

        #endregion

        #region Budget

        public Guid BudgetUsageCauseAggregateID { get; set; }
        public Guid BudgetUsageAggregateID { get; set; }
        public virtual CallBudgetUsageAggregate BudgetUsageAggregate { get; }
        public virtual CallBudgetUsageCauseAggregate BudgetUsageCauseAggregate { get; }

        #endregion 

        #region Priority

        public Guid PriorityID { get; set; }
        public virtual Priority Priority { get; }

        #endregion

        #region Calendar Work Schedule
        public Guid? CalendarWorkScheduleID { get; set; }        
        public virtual CalendarWorkSchedule CalendarWorkSchedule { get; }

        #endregion

        #region Time Zone

        public string TimeZoneID { get; set; }
        public virtual ServiceDesk.TimeZone TimeZone { get; }

        #endregion

        #region Negotiations

        public Negotiations.Negotiation CreateNegotiation()
        {
            return new Negotiations.CallNegotiation(IMObjID);
        }

        #endregion

        #region Referenced WorkOrders

        public string ReferenceName => $"IM-CL-{Number}";

        public WorkOrderReference CreateWorkOrderReference()
        {
            return new WorkOrderReference<Call>(this);
        }

        public virtual IEnumerable<WorkOrderReference<Call>> WorkOrderReferences { get; }

        #endregion

        #region Aggregate

        public virtual CallAggregate Aggregate { get; init; }       

        #endregion

        #region IMarkableForDelete

        public bool Removed { get; private set; }
        public void MarkForDelete() => Removed = true;

        #endregion

        #region Manhours

        public virtual IEnumerable<ManhoursWork> Manhours { get; }
        public void OnManhoursWorkAdded()
        {
            UtcDateModified = DateTime.UtcNow;
        }

        public void IncrementTotalManhours(int value)
        {
            ManhoursInMinutes += value;
            UtcDateModified = DateTime.UtcNow;
        }

        public void DecrementTotalManhours(int value)
        {
            ManhoursInMinutes -= value;
            UtcDateModified = DateTime.UtcNow;
        }

        public int ManhoursInMinutes { get; set; }

        #endregion

        public byte[] RowVersion { get; set; }        
    }
}
