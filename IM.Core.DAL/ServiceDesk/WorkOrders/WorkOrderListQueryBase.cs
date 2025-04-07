using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    internal class WorkOrderListQueryBase<TListQueryResultItem> : IWorkOrderListQueryBase<TListQueryResultItem> where TListQueryResultItem : WorkOrderListQueryResultItemBase, new()
    {
        private readonly CrossPlatformDbContext _db;

        public WorkOrderListQueryBase(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<TListQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<WorkOrder, bool>>> filterBy)
        {
            var workOrders = _db.Set<WorkOrder>().Where(filterBy);

            return from workOrder in workOrders
                   join sla in _db.Set<ServiceLevelAgreement>().AsNoTracking()
                       on workOrder.BudgetUsageCause.SlaID equals sla.ID
                           into slaLeftJoin
                   from sla in slaLeftJoin.DefaultIfEmpty()
                   join customControl in _db.Set<CustomControl>().AsNoTracking()
                       on new { UserId = userId, ObjectId = workOrder.IMObjID }
                           equals new { customControl.UserId, customControl.ObjectId }
                           into workOrderCustomControl
                   from customControl in workOrderCustomControl.DefaultIfEmpty()
                   join queueUser in _db.Set<GroupUser>().AsNoTracking()
                       on new { GroupId = workOrder.QueueID, UserID = userId }
                           equals new { GroupId = (Guid?)queueUser.GroupID, queueUser.UserID }
                           into workOrderQueueUser
                   from queueUser in workOrderQueueUser.DefaultIfEmpty()
                        join workOrderTypes in _db.Set<WorkOrderType>().AsNoTracking().IgnoreQueryFilters()
                        on workOrder.TypeID equals workOrderTypes.ID
                   select new TListQueryResultItem
                   {
                       ID = workOrder.IMObjID,
                       Number = workOrder.Number,
                       Name = workOrder.Name,
                       Description = DbFunctions.CastAsString(workOrder.Description),
                       UserField1 = workOrder.UserField1,
                       UserField2 = workOrder.UserField2,
                       UserField3 = workOrder.UserField3,
                       UserField4 = workOrder.UserField4,
                       UserField5 = workOrder.UserField5,
                       WorkflowSchemeID = workOrder.WorkflowSchemeID,
                       WorkflowSchemeIdentifier = workOrder.WorkflowSchemeIdentifier,
                       WorkflowSchemeVersion = workOrder.WorkflowSchemeVersion,
                       EntityStateID = DbFunctions.CastAsString(workOrder.EntityStateID),
                       EntityStateName = DbFunctions.CastAsString(workOrder.EntityStateName),
                       TypeID = workOrder.TypeID,
                       TypeName = workOrderTypes.Name,
                       PriorityID = workOrder.PriorityID,
                       PriorityName = workOrder.Priority.Name,
                       PriorityColor = workOrder.Priority.Color,
                       PrioritySequence = workOrder.Priority.Sequence,
                       InitiatorFullName = User.GetFullName(workOrder.InitiatorID),
                       InitiatorID = workOrder.InitiatorID,
                       ExecutorFullName = User.GetFullName(workOrder.ExecutorID),
                       ExecutorID = workOrder.ExecutorID,
                       AssigneeID = workOrder.AssigneeID,
                       QueueName = workOrder.Group.Name,
                       QueueID = workOrder.QueueID,
                       AssignorFullName = User.GetFullName(workOrder.AssigneeID),
                       UtcDateCreated = workOrder.UtcDateCreated,
                       UtcDateModified = workOrder.UtcDateModified,
                       UtcDateAssigned = workOrder.UtcDateAssigned,
                       UtcDateAccepted = workOrder.UtcDateAccepted,
                       UtcDateStarted = workOrder.UtcDateStarted,
                       UtcDateAccomplished = workOrder.UtcDateAccomplished,
                       UtcDatePromised = workOrder.UtcDatePromised,
                       Manhours = workOrder.ManhoursInMinutes,
                       ManhoursNorm = workOrder.ManhoursNormInMinutes,
                       BudgetUsageCauseString = workOrder.BudgetUsageCause.Name,
                       BudgetString = workOrder.BudgetUsage.FullName,
                       ReferencedObjectNumberString = workOrder.WorkOrderReference.ReferenceName,
                       DocumentCount = workOrder.Aggregate.DocumentCount,
                       IsFinished = workOrder.IsFinished,
                       IsOverdue = Workflow.Workflow.IsOverdue(ObjectClass.WorkOrder, workOrder.IMObjID),
                       UnreadMessageCount = ObjectNote.GetUnreadObjectNoteCount(workOrder.IMObjID, userId)
                   };
        }
    }
}
