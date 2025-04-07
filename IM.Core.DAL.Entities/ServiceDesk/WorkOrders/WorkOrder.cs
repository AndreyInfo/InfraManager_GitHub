using Inframanager;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System;
using System.Collections.Generic;
using InfraManager.DAL.ServiceDesk.Manhours;
using System.Linq.Expressions;
using InfraManager.Linq;

namespace InfraManager.DAL.ServiceDesk
{
    [ObjectClassMapping(ObjectClass.WorkOrder)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.WorkOrder_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.WorkOrder_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.WorkOrder_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]// не трогать
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]// не трогать
    public class WorkOrder : 
        IServiceDeskEntity, 
        ICreateNegotiation, 
        IHaveManhours, 
        IHaveUserFields,
        IHaveBudget
    {
        #region .ctor

        protected WorkOrder()
        {
        }

        public WorkOrder(Guid workOrderTypeId)
        {
            IMObjID = Guid.NewGuid();
            TypeID = workOrderTypeId;
            UtcDateCreated = DateTime.UtcNow;
            UtcDateModified = DateTime.UtcNow;
            Aggregate = new WorkOrderAggregate(IMObjID);
            UserField1 = string.Empty;
            UserField2 = string.Empty;
            UserField3 = string.Empty;
            UserField4 = string.Empty;
            UserField5 = string.Empty;
            HTMLDescription = string.Empty;
            Description = string.Empty;
            BudgetUsageAggregateID = Guid.Empty;
            BudgetUsageCauseAggregateID = Guid.Empty;
        }

        public WorkOrder(Guid workOrderTypeId, string concord, string bill, bool detailBudget) : this(workOrderTypeId)
        {
            FinancePurchase = new WorkOrderFinancePurchase(IMObjID, concord, bill, detailBudget);
        }

        #endregion

        #region Properties

        public Guid IMObjID { get; }
        public int Number { get; }
        public string Name { get; set; }
        public DateTime UtcDateCreated { get; set; }
        public DateTime UtcDateModified { get; set; }
        public DateTime? UtcDateAssigned { get; set; }
        public DateTime? UtcDateAccepted { get; set; }
        public DateTime? UtcDateStarted { get; set; }
        public DateTime UtcDatePromised { get; set; }
        public DateTime? UtcDateAccomplished { get; set; }
        public byte[] RowVersion { get; set; }

        #endregion

        #region FormValues

        public long? FormValuesID { get; set; }
        public Guid? FormID { get; set; }

        public virtual FormValues FormValues { get; set; }

        #endregion

        #region Type

        public Guid TypeID { get; set; }
        public virtual WorkOrderType Type { get; }

        #endregion

        #region Priority

        public Guid PriorityID { get; set; }
        public virtual WorkOrderPriority Priority { get; }

        #endregion

        #region Employees

        public Guid? InitiatorID { get; set; }
        public Guid InitiatorDefaultID { get; }
        public virtual User Initiator { get; }
        public Guid? ExecutorID { get; set; }
        public Guid ExecutorDefaultID { get; }
        public virtual User Executor { get; }
        public static Expression<Func<WorkOrder, string>> ExecutorFullName =>
            User.FullNameExpression.Substitute<WorkOrder, User, string>(x => x.Executor);
        public Guid? AssigneeID { get; set; }
        public Guid AssigneeDefaultID { get; }
        public virtual User Assignee { get; }
        public static Expression<Func<WorkOrder, string>> AssigneeFullName =>
            User.FullNameExpression.Substitute<WorkOrder, User, string>(x => x.Assignee);
        public Guid? QueueID { get; set; }
        public Guid QueueDefaultID { get; }
        public virtual Group Group { get; }

        #endregion

        #region User Fields

        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }
        public string UserField4 { get; set; }
        public string UserField5 { get; set; }

        #endregion

        #region Workflow

        public string EntityStateID { get; set; }
        public string EntityStateName { get; set; }
        public string TargetEntityStateID { get; set; }
        public Guid? WorkflowSchemeID { get; set; }
        public virtual WorkFlowScheme WorkflowScheme { get; }
        public string WorkflowSchemeIdentifier { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        public int IsActive { get; }
        public bool IsFinished { get; }
        public bool IsOverdue => UtcDatePromised < DateTime.UtcNow;

        #endregion

        #region Description

        public string HTMLDescription { get; set; }

        public string Description { get; set; }

        #endregion

        #region ManHours

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

        public int ManhoursNormInMinutes { get; set; }
        public int ManhoursInMinutes { get; set; }

        #endregion

        #region Finance

        public Guid BudgetUsageAggregateID { get; set; }
        public virtual CallBudgetUsageAggregate BudgetUsage { get; }
        public Guid BudgetUsageCauseAggregateID { get; set; }
        public virtual CallBudgetUsageCauseAggregate BudgetUsageCause { get; }
        public virtual WorkOrderFinancePurchase FinancePurchase { get; }

        #endregion

        #region WorkOrder reference
        public string ReferenceNumber => $"IM-TS-{Number}";

        public long WorkOrderReferenceID { get; private set; }

        private WorkOrderReference _workOrderReference;
        public virtual WorkOrderReference WorkOrderReference 
        {
            get
            {
                return _workOrderReference;
            }
            set
            {
                _workOrderReference = value;
                WorkOrderReferenceID = _workOrderReference.ID;
            }
        }

        #endregion

        #region Negotiations

        public Negotiation CreateNegotiation()
        {
            return new WorkOrderNegotiation(IMObjID);
        }

        #endregion

        #region Aggregate

        public virtual WorkOrderAggregate Aggregate { get; }

        #endregion
    }
}
