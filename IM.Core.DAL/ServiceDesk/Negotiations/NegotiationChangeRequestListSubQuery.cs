using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    internal class NegotiationChangeRequestListSubQuery : IListQuery<ChangeRequest, NegotiationListSubQueryResultItem>, ISelfRegisteredService<IListQuery<ChangeRequest, NegotiationListSubQueryResultItem>>
    {
        private readonly DbContext _db;

        public NegotiationChangeRequestListSubQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<NegotiationListSubQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<ChangeRequest, bool>>> filterBy)
        {
            IQueryable<ChangeRequest> changeRequests = _db.Set<ChangeRequest>().AsNoTracking();

            foreach (var filterExpression in filterBy)
            {
                changeRequests = changeRequests.Where(filterExpression);
            }

            return from changeRequest in changeRequests
                   let messagesCount = _db.Set<Note<ChangeRequest>>()
                    .Count(n => n.ParentObjectID == changeRequest.IMObjID && n.Type == SDNoteType.Message)
                   let notesCount = _db.Set<Note<ChangeRequest>>()
                    .Count(n => n.ParentObjectID == changeRequest.IMObjID && n.Type == SDNoteType.Note)
                   select new NegotiationListSubQueryResultItem
                   {
                       ObjectID = changeRequest.IMObjID,
                       Number = changeRequest.Number,
                       ClassID = ObjectClass.ChangeRequest,
                       CategorySortColumn = Issues.ChangeRequest,
                       Name = DbFunctions.CastAsString(changeRequest.Summary),
                       WorkflowSchemeIdentifier = changeRequest.WorkflowSchemeIdentifier,
                       WorkflowSchemeVersion = changeRequest.WorkflowSchemeVersion,
                       EntityStateName = changeRequest.EntityStateName,
                       EntityStateID = changeRequest.EntityStateID,
                       WorkflowSchemeID = changeRequest.WorkflowSchemeID,
                       PriorityName = changeRequest.Priority.Name,
                       PriorityColor = changeRequest.Priority.Color,
                       PriorityID = changeRequest.Priority.ID,
                       PrioritySequence = changeRequest.Priority.Sequence,
                       TypeFullName = DbFunctions.CastAsString(ProblemType.GetFullProblemTypeName(changeRequest.Type.ID)),
                       TypeID = changeRequest.Type.ID,
                       OwnerID = changeRequest.OwnerID,
                       ExecutorID = null,
                       QueueID = null,
                       UtcDateRegistered = changeRequest.UtcDateDetected,
                       UtcDateModified = changeRequest.UtcDateModified,
                       UtcDateClosed = changeRequest.UtcDateClosed,
                       UtcDatePromised = DbFunctions.CastAsDateTime(changeRequest.UtcDatePromised),
                       UtcDateAccomplished = changeRequest.UtcDateSolved,
                       ClientID = null,
                       ClientSubdivisionID = null,
                       ClientOrganizationID = null,
                       ClientFullName = null,
                       ClientSubdivisionFullName = null,
                       ClientOrganizationName = null,
                       CanBePicked = false,
                       IsFinished = changeRequest.UtcDateSolved != null,
                       NoteCount = notesCount,
                       MessagesCount = messagesCount
                   };
        }
    }
}


