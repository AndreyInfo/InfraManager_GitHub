using InfraManager.DAL.Documents;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    internal class NegotiationListQuery : IListQuery<Negotiation, NegotiationListSubQueryResultItem, NegotiationListQueryResultItem>, ISelfRegisteredService<IListQuery<Negotiation, NegotiationListSubQueryResultItem, NegotiationListQueryResultItem>>
    {
        private readonly DbContext _db;

        public NegotiationListQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<NegotiationListQueryResultItem> Query(Guid userID, IQueryable<NegotiationListSubQueryResultItem> subQuery, IEnumerable<Expression<Func<Negotiation, bool>>> filterBy)
        {
            IQueryable<Negotiation> negotiations = _db.Set<Negotiation>().AsNoTracking();
            var deputyUsers = _db.Set<DeputyUser>().Where(DeputyUser.UserIsDeputy(userID));

            foreach (var filterExpression in filterBy)
            {
                negotiations = negotiations.Where(filterExpression);
            }

            return from negotiation in negotiations
                   join sub in subQuery
                       on negotiation.ObjectID equals sub.ObjectID
                   join customControl in _db.Set<CustomControl>().AsNoTracking()
                      on new { UserId = userID, ObjectId = sub.ObjectID }
                      equals new { customControl.UserId, customControl.ObjectId }
                      into negotiationCustomControl
                   from customControl in negotiationCustomControl.DefaultIfEmpty()
                   join queue in _db.Set<Group>().AsNoTracking()
                       on sub.QueueID equals queue.IMObjID
                       into queueSub
                   from queue in queueSub.DefaultIfEmpty()
                   where negotiation.NegotiationUsers
                       .Any(n => n.UserID == userID
                           || deputyUsers.Any(x => x.ParentUserId == n.UserID))
                   let documentCount = _db.Set<DocumentReference>()
                   .AsNoTracking()
                   .Where(x => x.ObjectID == sub.ObjectID)
                   .Count()
                   let vote = negotiation.NegotiationUsers
                       .FirstOrDefault(n => n.UserID == userID
                            || deputyUsers.Any(x => x.ParentUserId == n.UserID))
                   select new NegotiationListQueryResultItem
                   {
                       ID = negotiation.IMObjID,
                       Number = sub.Number,
                       ClassID = sub.ClassID,
                       CategorySortColumn = sub.CategorySortColumn,
                       Name = sub.Name,
                       WorkflowSchemeIdentifier = sub.WorkflowSchemeIdentifier,
                       WorkflowSchemeVersion = sub.WorkflowSchemeVersion,
                       EntityStateName = sub.EntityStateName,
                       PriorityName = sub.PriorityName,
                       PriorityID = sub.PriorityID,
                       PriorityColor = sub.PriorityColor,
                       PrioritySequence = sub.PrioritySequence,
                       TypeFullName = sub.TypeFullName,
                       TypeID = sub.TypeID,
                       OwnerFullName = User.GetFullName(sub.OwnerID),
                       OwnerID = sub.OwnerID,
                       ExecutorFullName = User.GetFullName(sub.ExecutorID),
                       ExecutorID = sub.ExecutorID,
                       QueueName = queue.Name,
                       QueueID = queue.IMObjID,
                       UtcDateRegistered = sub.UtcDateRegistered,
                       UtcDateModified = sub.UtcDateModified,
                       UtcDateClosed = sub.UtcDateClosed,
                       UtcDatePromised = sub.UtcDatePromised,
                       UtcDateAccomplished = sub.UtcDateAccomplished,
                       ClientFullName = sub.ClientFullName,
                       ClientSubdivisionFullName = sub.ClientSubdivisionFullName,
                       ClientOrganizationName = sub.ClientOrganizationName,
                       UnreadMessageCount = ObjectNote.GetUnreadObjectNoteCount(negotiation.ObjectID, userID),
                       IsFinished = sub.IsFinished,
                       IsOverdue = sub.UtcDatePromised != null && sub.UtcDatePromised < DateTime.UtcNow,
                       DocumentCount = documentCount,
                       ObjectID = sub.ObjectID,
                       UtcDateVote = vote.UtcVoteDate,
                       UserVote = vote.VotingType,
                       NegotiationStatus = negotiation.Status,
                       NegotiationName = negotiation.Name,
                       NegotiationMode = negotiation.Mode,
                       UtcNegotiationDateVoteStart = negotiation.UtcDateVoteStart,
                       UtcNegotiationDateVoteEnd = negotiation.UtcDateVoteEnd,
                       InControl = customControl.ObjectId != null,
                       CanBePicked = sub.CanBePicked,
                       NoteCount = sub.NoteCount,
                       MessageCount = sub.MessagesCount
                   };

        }
    }
}


