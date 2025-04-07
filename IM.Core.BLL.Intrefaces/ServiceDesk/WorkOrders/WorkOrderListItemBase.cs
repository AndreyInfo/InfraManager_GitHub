using Inframanager.BLL;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.Settings.UserFields;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderListItemBase : PlugViewListItem, ISDEntityWithPriorityColorInt
    {
        public ObjectClass ClassID => ObjectClass.WorkOrder;

        [RangeSliderFilter(false)]
        [ColumnSettings(1)]
        [Label(nameof(Resources.NumberSymbol))]
        public int Number { get; init; }

        [LikeFilter]
        [ColumnSettings(2)]
        [Label(nameof(Resources.Name))]
        public string Name { get; init; }

        [LikeFilter]
        [ColumnSettings(3)]
        [Label(nameof(Resources.Description))]
        public string Description { get; init; }

        [LikeFilter]
        [ColumnSettings(25)]
        [UserFieldDisplay(UserFieldType.WorkOrder, FieldNumber.UserField1)]
        public string UserField1 { get; init; }

        [LikeFilter]
        [ColumnSettings(26)]
        [UserFieldDisplay(UserFieldType.WorkOrder, FieldNumber.UserField2)]
        public string UserField2 { get; init; }

        [LikeFilter]
        [ColumnSettings(27)]
        [UserFieldDisplay(UserFieldType.WorkOrder, FieldNumber.UserField3)]
        public string UserField3 { get; init; }

        [LikeFilter]
        [ColumnSettings(28)]
        [UserFieldDisplay(UserFieldType.WorkOrder, FieldNumber.UserField4)]
        public string UserField4 { get; init; }

        [LikeFilter]
        [ColumnSettings(29)]
        [UserFieldDisplay(UserFieldType.WorkOrder, FieldNumber.UserField5)]
        public string UserField5 { get; init; }

        [MultiSelectFilter(LookupQueries.WorkOrderStateName)]
        [ColumnSettings(6)]
        [Label(nameof(Resources.WorkOrderState))]
        public string EntityStateName { get; init; }

        public Guid TypeID { get; init; }

        [MultiSelectFilter(LookupQueries.WorkOrderType)]
        [ColumnSettings(7)]
        [Label(nameof(Resources.WorkOrderType))]
        public string TypeName { get; init; }

        public Guid PriorityID { get; init; }
        public int PriorityColor { get; init; }

        [MultiSelectFilter(LookupQueries.WorkOrderPriorities)]
        [ColumnSettings(8)]
        [ColumnSort(nameof(WorkOrderListQueryResultItemBase.PrioritySequence))]
        [Label(nameof(Resources.WorkOrderPriority))]
        public string PriorityName { get; init; }

        [SimpleValueFilter("WebUserSearcher", true)]
        [ColumnSettings(10)]
        [Label(nameof(Resources.WorkOrderInitiator))]
        public string InitiatorFullName { get; init; }

        public Guid? InitiatorID { get; init; }

        [SimpleValueFilter("ExecutorUserSearcher", true)]
        [ColumnSettings(11)]
        [Label(nameof(Resources.Executor))]
        public string ExecutorFullName { get; init; }

        public Guid? ExecutorID { get; init; }

        [SimpleValueFilter("QueueSearcher", true)]
        [ColumnSettings(30)]
        [Label(nameof(Resources.Queue))]
        public string QueueName { get; init; }

        public Guid? QueueID { get; init; }

        [SimpleValueFilter("AccomplisherUserSearcher", true)]
        [ColumnSettings(12)]
        [Label(nameof(Resources.Assignor))]
        public string AssignorFullName { get; init; }

        [DatePickFilter]
        [ColumnSettings(13)]
        [Label(nameof(Resources.WorkOrderDateCreated))]
        public DateTime UtcDateCreated { get; init; }

        [DatePickFilter]
        [ColumnSettings(14)]
        [Label(nameof(Resources.WorkOrderDateAssigned))]
        public DateTime? UtcDateAssigned { get; init; }

        [DatePickFilter]
        [ColumnSettings(15)]
        [Label(nameof(Resources.WorkOrderDateAccepted))]
        public DateTime? UtcDateAccepted { get; init; }

        [DatePickFilter]
        [ColumnSettings(16)]
        [Label(nameof(Resources.WorkOrderDatePromise))]
        public DateTime UtcDatePromised { get; init; }

        [DatePickFilter]
        [ColumnSettings(19)]
        [Label(nameof(Resources.WorkOrderDateStarted))]
        public DateTime? UtcDateStarted { get; init; }

        [DatePickFilter]
        [ColumnSettings(20)]
        [Label(nameof(Resources.WorkOrderDateAccomplished))]
        public DateTime? UtcDateAccomplished { get; init; }

        [DatePickFilter]
        [ColumnSettings(21)]
        [Label(nameof(Resources.WorkOrderDateModified))]
        public DateTime UtcDateModified { get; init; }

        [ColumnSettings(4)]
        [Label(nameof(Resources.ManhoursListCaption))]
        [ColumnSort(nameof(WorkOrderListQueryResultItemBase.Manhours))]
        public string ManhoursInMinutes { get; init; }

        [ColumnSettings(5)]
        [Label(nameof(Resources.ManhoursNorm))]
        [ColumnSort(nameof(WorkOrderListQueryResultItemBase.ManhoursNorm))]
        public string ManhoursNormInMinutes { get; init; }

        [MultiSelectFilter(LookupQueries.WorkOrderBudgetUsageCause)]
        [ColumnSettings(23)]
        [Label(nameof(Resources.BudgetGround))]
        public string BudgetUsageCauseString { get; init; }

        [MultiSelectFilter(LookupQueries.WorkOrderBudget)]
        [ColumnSettings(22)]
        [Label(nameof(Resources.BudgetName))]
        public string BudgetString { get; init; }

        [ColumnSettings(24)]
        [Label(nameof(Resources.WorkOrderReference))]
        public string ReferencedObjectNumberString { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(9, 50)]
        [Label(nameof(Resources.Attachments))]
        public int DocumentCount { get; init; }

        [ColumnSettings(17, 50)]
        [Label(nameof(Resources.WorkOrderIsFinished))]
        public bool IsFinished { get; init; }

        [ColumnSettings(18, 50)]
        [Label(nameof(Resources.WorkOrderIsOverdue))]
        public bool IsOverdue { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(0, 50)]
        [Label(nameof(Resources.NewMessages))]
        public int UnreadMessageCount { get; init; }

        public Guid? AssigneeID { get; init; }
    }
}
