using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.Finance;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.DAL.ServiceDesk
{
    internal class AllCallsQuery : 
        IListQuery<Call, AllCallsQueryResultItem>, 
        ISelfRegisteredService<IListQuery<Call, AllCallsQueryResultItem>>
    {
        private readonly DbContext _db;
        public AllCallsQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<AllCallsQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<Call, bool>>> filterBy)
        {
            var calls = _db.Set<Call>().Where(filterBy);

            var query = from call in calls
                        join subdivision in _db.Set<Subdivision>().AsNoTracking()
                        on call.ClientSubdivisionID equals subdivision.ID
                        into callSubdivision
                        from subdivision in callSubdivision.DefaultIfEmpty()

                        join budgetUsageCause in _db.Set<BudgetUsageCause>().AsNoTracking()
                        on call.IMObjID equals budgetUsageCause.ObjectId
                        into callBudgetUsageCause
                        from budgetUsageCause in callBudgetUsageCause.DefaultIfEmpty() // TODO: reuse existing query

                        join serviceLevelAgreement in _db.Set<ServiceLevelAgreement>().AsNoTracking()
                        on budgetUsageCause.SLAID equals serviceLevelAgreement.ID
                        into slaBudgetUsageCause
                        from serviceLevelAgreement in slaBudgetUsageCause.DefaultIfEmpty() // TODO: reuse existing query

                        join budgetUsageAggregate in _db.Set<CallBudgetUsageAggregate>().AsNoTracking()
                        on call.BudgetUsageAggregateID equals budgetUsageAggregate.ID
                        into callBudgetUsageAggregate
                        from budgetUsageAggregate in callBudgetUsageAggregate.DefaultIfEmpty()

                        join customControl in _db.Set<CustomControl>().AsNoTracking()
                        on new { UserId = userId, ObjectId = call.IMObjID }
                        equals new { customControl.UserId, customControl.ObjectId }
                        into callCustomControl
                        from customControl in callCustomControl.DefaultIfEmpty()

                        join queueUser in _db.Set<GroupUser>().AsNoTracking()
                        on new { GroupId = call.QueueID, UserID = userId }
                        equals new { GroupId = (Guid?)queueUser.GroupID, queueUser.UserID }
                        into callQueueUser
                        from queueUser in callQueueUser.DefaultIfEmpty()

                        join budgetUsageCauseAggregate in _db.Set<CallBudgetUsageCauseAggregate>().AsNoTracking()
                        on call.BudgetUsageCauseAggregateID equals budgetUsageCauseAggregate.ID
                        into callBudgetUsageCauseAggregateID
                        from budgetUsageCauseAggregate in callBudgetUsageCauseAggregateID

                        let problemCount = _db.Set<CallReference>()
                        .Where(x => x.CallID == call.IMObjID && x.ObjectClassID == ObjectClass.Call)
                        .Count()

                        select new AllCallsQueryResultItem
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
                            Manhours = call.ManhoursInMinutes,
                            ManhoursNorm = call.ManhoursNormInMinutes,
                            UserField1 = call.UserField1,
                            UserField2 = call.UserField2,
                            UserField3 = call.UserField3,
                            UserField4 = call.UserField4,
                            UserField5 = call.UserField5,
                            WorkflowSchemeID = call.WorkflowSchemeID,
                            WorkflowSchemeIdentifier = call.WorkflowSchemeIdentifier,
                            WorkflowSchemeVersion = call.WorkflowSchemeVersion,
                            EntityStateID = call.EntityStateID,
                            EntityStateName = call.EntityStateName,
                            InitiatorFullName = User.GetFullName(call.InitiatorID),
                            OwnerID = call.OwnerID,
                            OwnerFullName = User.GetFullName(call.OwnerID),
                            AccomplisherFullName = User.GetFullName(call.AccomplisherID),
                            AccomplisherID = call.AccomplisherID,
                            ClientFullName = User.GetFullName(call.ClientID),
                            ClientSubdivisionFullName = Subdivision.GetFullSubdivisionName(call.ClientSubdivisionID),
                            ClientOrganizationName = subdivision.Organization.Name,
                            ServiceName = call.CallService.ServiceName,
                            ServiceItemOrAttendance = call.CallService.ServiceItemOrAttendanceName,
                            TypeID = call.CallTypeID,
                            TypeFullName = CallType.GetFullCallTypeName(call.CallTypeID),
                            ExecutorFullName = User.GetFullName(call.ExecutorID),
                            ExecutorID = call.ExecutorID,
                            InitiatorID = call.InitiatorID,
                            QueueName = call.Aggregate.QueueName,
                            QueueID = call.QueueID,
                            UrgencyName = call.Urgency.Name,
                            InfluenceName = call.Influence.Name,
                            PriorityID = call.Priority.ID,
                            PriorityName = call.Priority.Name,
                            PriorityColor = call.Priority.Color,
                            PrioritySequence = call.Priority.Sequence,
                            UtcDateModified = call.UtcDateModified,
                            UtcDateRegistered = call.UtcDateRegistered,
                            UtcDateOpened = call.UtcDateOpened,
                            UtcDatePromised = call.UtcDatePromised,
                            UtcDateAccomplished = call.UtcDateAccomplished,
                            UtcDateClosed = call.UtcDateClosed,
                            UtcDateCreated = call.UtcDateCreated,
                            DocumentCount = call.Aggregate.DocumentCount,
                            WorkOrderCount = call.Aggregate.WorkOrderCount,
                            ProblemCount = problemCount,
                            BudgetUsageCauseString = budgetUsageCauseAggregate.Name,
                            BudgetString = budgetUsageAggregate.FullName,
                            UnreadMessageCount = ObjectNote.GetUnreadObjectNoteCount(call.IMObjID, userId),
                            IsFinished = Workflow.Workflow.IsFinished(ObjectClass.Call, call.IMObjID),
                            IsOverdue = Workflow.Workflow.IsOverdue(ObjectClass.Call, call.IMObjID),
                            ClientID = call.ClientID,
                            ClientEmail = call.Client.Email
                        };

            return query;
        }
    }
}
