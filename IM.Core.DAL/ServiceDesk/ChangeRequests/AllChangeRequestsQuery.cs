using InfraManager;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Documents;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal class AllChangeRequestsQuery : 
        IListQuery<ChangeRequest, ChangeRequestQueryResultItem>, 
        ISelfRegisteredService<IListQuery<ChangeRequest, ChangeRequestQueryResultItem>>
    {
        private readonly DbContext _db;
        public AllChangeRequestsQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<ChangeRequestQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<ChangeRequest, bool>>> filterBy)
        {
            var changeRequests = _db.Set<ChangeRequest>().Where(filterBy);

            var reasonObjects = _db.Set<Call>().AsNoTracking().Select(c => new { ID = c.IMObjID, ClassID = ObjectClass.Call, Name = "IM-CL-" + DbFunctions.CastAsString(c.Number) })
                    .Union(_db.Set<WorkOrder>().AsNoTracking().Select(wo => new { ID = wo.IMObjID, ClassID = ObjectClass.WorkOrder, Name = "IM-TS-" + DbFunctions.CastAsString(wo.Number) }))
                    .Union(_db.Set<Problem>().AsNoTracking().Select(p => new { ID = p.IMObjID, ClassID = ObjectClass.Problem, Name = "IM-PL-" + DbFunctions.CastAsString(p.Number) }))
                    .Union(_db.Set<ChangeRequest>().AsNoTracking().Select(rfc => new { ID = rfc.IMObjID, ClassID = ObjectClass.ChangeRequest, Name = "IM-RF-" + DbFunctions.CastAsString(rfc.Number) }));

            var query = from changeRequest in changeRequests
                        join urgency in _db.Set<Urgency>().AsNoTracking()
                        on changeRequest.UrgencyID equals urgency.ID
                        into changeRequestUrgency
                        from urgency in changeRequestUrgency.DefaultIfEmpty()

                        join influence in _db.Set<Influence>().AsNoTracking()
                        on changeRequest.InfluenceID equals influence.ID
                        into changeRequestInfluence
                        from influence in changeRequestInfluence.DefaultIfEmpty()

                        join changeRequestCategory in _db.Set<ChangeRequestCategory>().AsNoTracking()
                        on changeRequest.CategoryID equals changeRequestCategory.ID
                        into categoryChangeRequest
                        from changeRequestCategory in categoryChangeRequest.DefaultIfEmpty()

                        join reasonObject in reasonObjects
                            on new { ID = changeRequest.ReasonObjectID, ClassID = changeRequest.ReasonObjectClassID } 
                                equals new { ID = (Guid?)reasonObject.ID, ClassID = (ObjectClass?)reasonObject.ClassID }
                            into reasonObjectReference 
                            from reason in reasonObjectReference.DefaultIfEmpty()

                        let workOrderCount = _db.Set<WorkOrderReference>()
                        .AsNoTracking()
                        .Where(x => x.ObjectID == changeRequest.IMObjID && x.ObjectClassID == ObjectClass.ChangeRequest)
                        .Count()

                        let documentCount = _db.Set<DocumentReference>()
                        .AsNoTracking()
                        .Where(x => x.ObjectID == changeRequest.IMObjID)
                        .Count()

                        select new ChangeRequestQueryResultItem
                        {
                            ID = changeRequest.IMObjID,
                            Number = changeRequest.Number,
                            Summary = changeRequest.Summary,
                            Description = DbFunctions.CastAsString(changeRequest.Description),
                            Manhours = changeRequest.ManhoursInMinutes,
                            ManhoursNorm = changeRequest.ManhoursNormInMinutes,
                            UrgencyName = urgency.Name,
                            UrgencySequence = urgency.Sequence,
                            Target = changeRequest.Target,
                            FundingAmount = changeRequest.FundingAmount,
                            InfluenceName = influence.Name,
                            InfluenceSequence = influence.Sequence,
                            UtcDateDetected = changeRequest.UtcDateDetected,
                            UtcDatePromised = changeRequest.UtcDatePromised,
                            UtcDateSolved = changeRequest.UtcDateSolved,
                            UtcDateClosed = changeRequest.UtcDateClosed,
                            UtcDateStarted = changeRequest.UtcDateStarted,
                            UtcDateModified = changeRequest.UtcDateModified,
                            PriorityID = changeRequest.Priority.ID,
                            PriorityName = changeRequest.Priority.Name,
                            PrioritySequence = changeRequest.Priority.Sequence,
                            TypeID = changeRequest.Type.ID,
                            TypeFullName = changeRequest.Type.Name,
                            OwnerID = changeRequest.OwnerID,
                            OwnerFullName = User.GetFullName(changeRequest.OwnerID),
                            InitiatorID = changeRequest.InitiatorID,
                            InitiatorFullName = User.GetFullName(changeRequest.InitiatorID),
                            IsFinished = Workflow.Workflow.IsFinished(ObjectClass.ChangeRequest, changeRequest.IMObjID),
                            CategoryID = changeRequest.CategoryID,
                            CategoryName = changeRequestCategory.Name,
                            IsOverdue = Workflow.Workflow.IsOverdue(ObjectClass.ChangeRequest, changeRequest.IMObjID),
                            UnreadMessageCount = ObjectNote.GetUnreadObjectNoteCount(changeRequest.IMObjID, userId),
                            NoteCount = ObjectNote.GetObjectNoteCount(ObjectClass.ChangeRequest, changeRequest.IMObjID, 0),
                            MessageCount = ObjectNote.GetObjectNoteCount(ObjectClass.ChangeRequest, changeRequest.IMObjID, 1),
                            WorkOrderCount = workOrderCount,
                            WorkflowSchemeIdentifier = changeRequest.WorkflowSchemeIdentifier,
                            WorkflowSchemeVersion = changeRequest.WorkflowSchemeVersion,
                            EntityStateName = changeRequest.EntityStateName,
                            DocumentCount = documentCount,
                            ReasonObjectName = reason.Name,
                            OnWorkOrderExecutorControl = changeRequest.OnWorkOrderExecutorControl,
                        };
            return query;
        }
    }
}
