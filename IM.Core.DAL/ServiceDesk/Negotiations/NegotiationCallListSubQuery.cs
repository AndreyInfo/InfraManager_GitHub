using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    internal class NegotiationCallListSubQuery : IListQuery<Call, NegotiationListSubQueryResultItem>, ISelfRegisteredService<IListQuery<Call, NegotiationListSubQueryResultItem>>
    {
        private readonly DbContext _db;

        public NegotiationCallListSubQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<NegotiationListSubQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<Call, bool>>> filterBy)
        {
            IQueryable<Call> calls = _db.Set<Call>().AsNoTracking();

            foreach (var filterExpression in filterBy)
            {
                calls = calls.Where(filterExpression);
            }

                return  from call in calls
                        join queueUser in _db.Set<GroupUser>().AsNoTracking()
                            on new { GroupId = call.QueueID, UserID = userId }
                            equals new { GroupId = (Guid?)queueUser.GroupID, queueUser.UserID }
                            into callQueueUser
                        from queueUser in callQueueUser.DefaultIfEmpty()
                        where !call.Removed //TODO: Consider whether to define QueryFilter for entity Call at ef configuration level
                        let messagesCount = _db.Set<Note<Call>>()
                            .Count(n => n.ParentObjectID == call.IMObjID && n.Type == SDNoteType.Message)
                        let notesCount = _db.Set<Note<Call>>()
                         .Count(n => n.ParentObjectID == call.IMObjID && n.Type == SDNoteType.Note)
                        select new NegotiationListSubQueryResultItem
                        {
                            ObjectID = call.IMObjID,
                            Number = call.Number,
                            ClassID = ObjectClass.Call,
                            CategorySortColumn = Issues.Call,
                            Name = DbFunctions.CastAsString(call.CallSummaryName),
                            WorkflowSchemeIdentifier = call.WorkflowSchemeIdentifier,
                            WorkflowSchemeVersion = call.WorkflowSchemeVersion,
                            EntityStateName = call.EntityStateName,
                            EntityStateID = call.EntityStateID,
                            WorkflowSchemeID = call.WorkflowSchemeID,
                            PriorityName = call.Priority.Name,
                            PriorityColor = call.Priority.Color,
                            PriorityID = call.Priority.ID,
                            PrioritySequence = call.Priority.Sequence,
                            TypeFullName = DbFunctions.CastAsString(CallType.GetFullCallTypeName(call.CallType.ID)),
                            TypeID = call.CallType.ID,
                            OwnerID = call.OwnerID,
                            ExecutorID = call.ExecutorID,
                            QueueID = call.QueueID,
                            UtcDateRegistered = call.UtcDateRegistered,
                            UtcDateModified = call.UtcDateModified,
                            UtcDateClosed = call.UtcDateClosed,
                            UtcDatePromised = DbFunctions.CastAsDateTime(call.UtcDatePromised),
                            UtcDateAccomplished = call.UtcDateAccomplished,
                            ClientID = call.ClientID,
                            ClientSubdivisionID = call.ClientSubdivisionID,
                            ClientOrganizationID = call.ClientSubdivision.Organization.ID,
                            ClientFullName = User.GetFullName(call.ClientID),
                            ClientSubdivisionFullName = Subdivision.GetFullSubdivisionName(call.ClientSubdivisionID),
                            ClientOrganizationName = DbFunctions.CastAsString(call.ClientSubdivision.Organization.Name),
                            CanBePicked = call.QueueID != null && queueUser.UserID != null && call.ExecutorID == null,
                            IsFinished = call.IsFinished,
                            MessagesCount = messagesCount,
                            NoteCount = notesCount
                        };

        }
    }
}


