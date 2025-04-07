using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderPredicateBuildersBase<TListItem> :
         FilterBuildersAggregate<WorkOrder, TListItem> where TListItem : WorkOrderListItemBase
    {
        public WorkOrderPredicateBuildersBase() : base()
        {
            AddPredicateBuilder(nameof(WorkOrderListItemBase.Name), x => x.Name);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.Number), x => x.Number);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.Description), x => DbFunctions.CastAsString(x.Description));
            AddPredicateBuilder(nameof(WorkOrderListItemBase.TypeName), x => x.Type.ID);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.InitiatorFullName), x => x.InitiatorID);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.AssignorFullName), x => x.AssigneeID);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UtcDateCreated), x => x.UtcDateCreated);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UtcDateAssigned), x => x.UtcDateAssigned);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UtcDateAccepted), x => x.UtcDateAccepted);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UtcDateStarted), x => x.UtcDateStarted);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UserField1), x => x.UserField1);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UserField2), x => x.UserField2);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UserField3), x => x.UserField3);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UserField4), x => x.UserField4);
            AddPredicateBuilder(nameof(WorkOrderListItemBase.UserField5), x => x.UserField5);
        }
    }
}
