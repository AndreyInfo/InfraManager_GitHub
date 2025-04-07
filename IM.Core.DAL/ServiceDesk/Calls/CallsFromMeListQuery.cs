using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Documents;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk.Calls;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    internal class CallsFromMeListQuery :
        IListQuery<Call, CallsFromMeListQueryResultItem>,
        ISelfRegisteredService<IListQuery<Call, CallsFromMeListQueryResultItem>>
    {
        private readonly DbContext _db;

        public CallsFromMeListQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<CallsFromMeListQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<Call, bool>>> filterBy)
        {
            var calls = _db.Set<Call>().Where(filterBy);
            var deputyUsers = _db.Set<DeputyUser>().AsNoTracking();

            var currentUser = _db.Set<User>().AsNoTracking()
                .Include(u => u.Subdivision)
                .SingleOrDefault(x => x.IMObjID == userId);
            var currentUserDivisionID = currentUser?.Subdivision?.ID;

            var showCallsForSubdivisionInWeb = _db.Set<UserRole>().AsNoTracking()
                .Include(ur => ur.Role)
                .ThenInclude(r => r.Operations)
                .FirstOrDefault(ur => ur.UserID == userId
                                    && ur.Role.Operations.Where(o => o.OperationID == OperationID.Call_ShowCallsForSubdivisionInWeb).Any())
                is not null;

            return from call in calls
                   join queueUser in _db.Set<GroupUser>().AsNoTracking()
                            on new { GroupId = call.QueueID, UserID = userId }
                            equals new { GroupId = (Guid?)queueUser.GroupID, queueUser.UserID }
                            into callQueueUser
                   from queueUser in callQueueUser.DefaultIfEmpty()
                   join customControl in _db.Set<CustomControl>().AsNoTracking()
                           on new { UserId = userId, ObjectId = call.IMObjID }
                           equals new { customControl.UserId, customControl.ObjectId }
                           into callCustomControl
                   from customControl in callCustomControl.DefaultIfEmpty()
                   where call.ClientID == userId
                    || call.InitiatorID == userId
                    || deputyUsers.Where(DeputyUser.IsActive).Any(dp => dp.ChildUserId == userId && call.ClientID == dp.ParentUserId)
                    || deputyUsers.Where(DeputyUser.IsActive).Any(dp => dp.ChildUserId == userId && call.InitiatorID == dp.ParentUserId)
                    || (showCallsForSubdivisionInWeb
                        && (call.OwnerID != userId || call.OwnerID == null)
                        && (call.ExecutorID != userId || call.ExecutorID == null)
                        && Subdivision.SubdivisionIsSibling(currentUserDivisionID, call.ClientSubdivisionID))
                   let documentCount = _db.Set<DocumentReference>()
                       .AsNoTracking()
                       .Count(x => x.ObjectID == call.IMObjID)
                   select new CallsFromMeListQueryResultItem
                   {
                       ID = call.IMObjID,
                       Number = call.Number,
                       ReceiptType = call.ReceiptType,
                       Summary = call.CallSummaryName,
                       Description = DbFunctions.CastAsString(call.Description),
                       Grade = call.Grade,
                       Solution = DbFunctions.CastAsString(call.Solution),
                       SLAName = call.SLAName,
                       Price = call.Price,
                       WorkflowSchemeID = call.WorkflowSchemeID,
                       WorkflowSchemeIdentifier = call.WorkflowSchemeIdentifier,
                       WorkflowSchemeVersion = call.WorkflowSchemeVersion,
                       EntityStateID = DbFunctions.CastAsString(call.EntityStateID),
                       EntityStateName = DbFunctions.CastAsString(call.EntityStateName),
                       InitiatorFullName = User.GetFullName(call.InitiatorID),
                       ClientFullName = User.GetFullName(call.ClientID),
                       ClientSubdivisionFullName = call.ClientSubdivision.Name,
                       ClientOrganizationName = call.ClientSubdivision.Organization.Name,
                       ServiceName = call.CallService.Service.Name,
                       ServiceItemOrAttendance = call.CallService.ServiceItem.Name ?? call.CallService.ServiceAttendance.Name ?? string.Empty,
                       TypeID = call.CallTypeID,
                       TypeFullName = DbFunctions.CastAsString(CallType.GetFullCallTypeName(call.CallTypeID)),
                       UrgencySequence = call.Urgency.Sequence,
                       UrgencyName = call.Urgency.Name,
                       UtcDateModified = call.UtcDateModified,
                       UtcDateRegistered = call.UtcDateRegistered,
                       UtcDatePromised = call.UtcDatePromised,
                       UtcDateAccomplished = call.UtcDateAccomplished,
                       UtcDateClosed = call.UtcDateClosed,
                       UtcDateCreated = call.UtcDateCreated,
                       DocumentCount = documentCount,
                       UnreadMessageCount = ObjectNote.GetUnreadObjectNoteCount(call.IMObjID, userId),
                       NoteCount = 0,
                       MessageCount = 0,
                       InControl = customControl.ObjectId != null,
                       CanBePicked = call.QueueID != null && queueUser.UserID != null && call.ExecutorID == null,
                       OwnerID = call.OwnerID,
                       OwnerFullName = User.GetFullName(call.OwnerID),
                       ClientID = call.ClientID,
                       ExecutorID = call.ExecutorID,
                       PriorityColor = call.Priority.Color
                   };
        }
    }
}