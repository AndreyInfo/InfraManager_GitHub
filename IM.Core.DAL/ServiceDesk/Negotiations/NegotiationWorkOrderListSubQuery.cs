using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    internal class NegotiationWorkOrderListSubQuery : IListQuery<WorkOrder, NegotiationListSubQueryResultItem>, ISelfRegisteredService<IListQuery<WorkOrder, NegotiationListSubQueryResultItem>>
    {
        private readonly DbContext _db;

        public NegotiationWorkOrderListSubQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<NegotiationListSubQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<WorkOrder, bool>>> filterBy)
        {
            IQueryable<WorkOrder> workOrders = _db.Set<WorkOrder>().AsNoTracking();

            foreach (var filterExpression in filterBy)
            {
                workOrders = workOrders.Where(filterExpression);
            }

               return   from workOrder in workOrders
                        join queueUser in _db.Set<GroupUser>().AsNoTracking()
                            on new { GroupId = workOrder.QueueID, UserID = userId }
                            equals new { GroupId = (Guid?)queueUser.GroupID, queueUser.UserID }
                            into workOrderQueueUser
                        from queueUser in workOrderQueueUser.DefaultIfEmpty()
                        let messagesCount = _db.Set<Note<WorkOrder>>()
                            .Count(n => n.ParentObjectID == workOrder.IMObjID && n.Type == SDNoteType.Message)
                        let notesCount = _db.Set<Note<WorkOrder>>()
                         .Count(n => n.ParentObjectID == workOrder.IMObjID && n.Type == SDNoteType.Note)
                        select new NegotiationListSubQueryResultItem
                        {
                            ObjectID = workOrder.IMObjID,
                            Number = workOrder.Number,
                            ClassID = ObjectClass.WorkOrder,
                            CategorySortColumn = Issues.WorkOrder,
                            Name = DbFunctions.CastAsString(workOrder.Name),
                            WorkflowSchemeIdentifier = workOrder.WorkflowSchemeIdentifier,
                            WorkflowSchemeVersion = workOrder.WorkflowSchemeVersion,
                            EntityStateName = workOrder.EntityStateName,
                            EntityStateID = workOrder.EntityStateID,
                            WorkflowSchemeID = workOrder.WorkflowSchemeID,
                            PriorityName = workOrder.Priority.Name,
                            PriorityColor = workOrder.Priority.Color,
                            PriorityID = workOrder.Priority.ID,
                            PrioritySequence = workOrder.Priority.Sequence,
                            TypeFullName = DbFunctions.CastAsString(workOrder.Type.Name),
                            TypeID = workOrder.Type.ID,
                            OwnerID = workOrder.AssigneeID,
                            ExecutorID = workOrder.ExecutorID,
                            QueueID = workOrder.QueueID,
                            UtcDateRegistered = workOrder.UtcDateAssigned,
                            UtcDateModified = workOrder.UtcDateModified,
                            UtcDateClosed = workOrder.UtcDateAccomplished,
                            UtcDatePromised = DbFunctions.CastAsDateTime(workOrder.UtcDatePromised),
                            UtcDateAccomplished = workOrder.UtcDateAccomplished,
                            ClientID = workOrder.InitiatorID,
                            ClientSubdivisionID = workOrder.Initiator.Subdivision.ID,
                            ClientOrganizationID = workOrder.Initiator.Subdivision.Organization.ID,
                            ClientFullName = User.GetFullName(workOrder.InitiatorID),
                            ClientSubdivisionFullName = Subdivision.GetFullSubdivisionName(workOrder.Initiator.Subdivision.ID),
                            ClientOrganizationName = DbFunctions.CastAsString(workOrder.Initiator.Subdivision.Organization.Name),
                            CanBePicked = workOrder.QueueID != null && queueUser.UserID != null && workOrder.ExecutorID == null,
                            IsFinished = workOrder.IsFinished,
                            MessagesCount = messagesCount,
                            NoteCount = notesCount
                        };

        }
    }
}


