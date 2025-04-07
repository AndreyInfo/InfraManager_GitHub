using InfraManager;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.Finance;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Documents;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal class ProblemListQuery :
        IListQuery<Problem, ProblemListQueryResultItem>,
        ISelfRegisteredService<IListQuery<Problem, ProblemListQueryResultItem>>
    {
        private readonly CrossPlatformDbContext _db;
        private readonly IBudgetObjectsQuery _budgetObjects;

        public ProblemListQuery(
            CrossPlatformDbContext db, 
            IBudgetObjectsQuery budgetObjects)
        {
            _db = db;
            _budgetObjects = budgetObjects;
        }

        public IQueryable<ProblemListQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<Problem, bool>>> filterBy)
        {
            var problems = _db.Set<Problem>().Where(filterBy);

            var query = from problem in problems
                        join budgetUsageCause in _db.Set<BudgetUsageCause>().AsNoTracking()
                         on new { ObjectId = problem.IMObjID, ObjectClass = ObjectClass.Problem }
                             equals new { budgetUsageCause.ObjectId, budgetUsageCause.ObjectClass }
                             into buc
                        from budgetUsageCause in buc.DefaultIfEmpty() // left join BudgetUsageCause
                        join budgetUsage in _db.Set<BudgetUsage>().AsNoTracking()
                         on new { ObjectId = problem.IMObjID, ObjectClass = ObjectClass.Problem }
                             equals new { budgetUsage.ObjectId, budgetUsage.ObjectClass }
                             into bu
                        from budgetUsage in bu.DefaultIfEmpty() // left join BudgetUsage
                        join customControl in _db.Set<CustomControl>().AsNoTracking()
                         on new { UserId = userId, ObjectId = problem.IMObjID }
                             equals new { customControl.UserId, customControl.ObjectId }
                             into cc
                        from customControl in cc.DefaultIfEmpty() // left join CustomControl
                        join budgetObject in _budgetObjects.Query()
                         on budgetUsage.BudgetObjectId equals budgetObject.ID
                         into bo
                        from budgetObject in bo.DefaultIfEmpty()
                        join owner in _db.Set<User>().AsNoTracking()
                         on problem.OwnerID equals owner.IMObjID
                         into users
                        from owner in users.DefaultIfEmpty()
                        let documentCount = _db
                            .Set<DocumentReference>()
                            .AsNoTracking()
                            .Count(x => x.ObjectID == problem.IMObjID)
                        let workOrderCount = _db
                            .Set<WorkOrderReference>()
                            .AsNoTracking()
                            .Count(x => x.ObjectID == problem.IMObjID && x.ObjectClassID == ObjectClass.Problem)
                        let callsCount = _db
                            .Set<CallReference>()
                            .AsNoTracking()
                            .Count(x => x.ObjectID == problem.IMObjID && x.ObjectClassID == ObjectClass.Problem)
                        select new ProblemListQueryResultItem
                        {
                            ID = problem.IMObjID,
                            Number = problem.Number,
                            Summary = problem.Summary,
                            Description = DbFunctions.CastAsString(problem.Description),
                            Solution = DbFunctions.CastAsString(problem.Solution),
                            Cause = DbFunctions.CastAsString(problem.Cause),
                            Fix = DbFunctions.CastAsString(problem.Fix),
                            Manhours = problem.ManhoursInMinutes,
                            ManhoursNorm = problem.ManhoursNormInMinutes,
                            UtcDateDetected = problem.UtcDateDetected,
                            UtcDatePromised = problem.UtcDatePromised,
                            UtcDateClosed = problem.UtcDateClosed,
                            UtcDateSolved = problem.UtcDateSolved,
                            UtcDateModified = problem.UtcDateModified,
                            UrgencyName = problem.Urgency.Name,
                            UrgencySequence = problem.Urgency.Sequence,
                            InfluenceName = problem.Influence.Name,
                            InfluenceSequence = problem.Influence.Sequence,
                            PriorityID = problem.Priority.ID,
                            PriorityName = problem.Priority.Name,
                            PriorityColor = problem.Priority.Color,
                            PrioritySequence = problem.Priority.Sequence,
                            ProblemCauseName = problem.ProblemCause.Name,
                            TypeID = problem.TypeID,
                            TypeFullName = ProblemType.GetFullProblemTypeName(problem.TypeID),
                            OwnerID = problem.OwnerID,
                            OwnerFullName = User.GetFullName(problem.OwnerID),
                            UserField1 = problem.UserField1,
                            UserField2 = problem.UserField2,
                            UserField3 = problem.UserField3,
                            UserField4 = problem.UserField4,
                            UserField5 = problem.UserField5,
                            WorkflowSchemeID = problem.WorkflowSchemeID,
                            WorkflowSchemeIdentifier = problem.WorkflowSchemeIdentifier,
                            WorkflowSchemeVersion = problem.WorkflowSchemeVersion,
                            EntityStateID = DbFunctions.CastAsString(problem.EntityStateID),
                            EntityStateName = DbFunctions.CastAsString(problem.EntityStateName),
                            DocumentCount = documentCount,
                            WorkOrderCount = workOrderCount,
                            CallCount = callsCount,
                            BudgetString = budgetUsage.BudgetId == null
                             ? budgetObject.Name
                             : BudgetUsage.GetBudgetFullName(budgetUsage.BudgetId),
                            BudgetUsageCauseString = budgetUsageCause.Sla.ID == null
                             ? budgetUsageCause.Text
                             : budgetUsageCause.Sla.Name,
                            UnreadMessageCount = ObjectNote.GetUnreadObjectNoteCount(problem.IMObjID, userId),
                            IsFinished = Workflow.Workflow.IsFinished(ObjectClass.Problem, problem.IMObjID),
                            IsOverdue = Workflow.Workflow.IsOverdue(ObjectClass.Problem, problem.IMObjID),
                            NoteCount = ObjectNote.GetObjectNoteCount(ObjectClass.Problem, problem.IMObjID, 0),
                            MessageCount = ObjectNote.GetObjectNoteCount(ObjectClass.Problem, problem.IMObjID, 1),
                            InControl = customControl.ObjectId != null,
                            OnWorkOrderExecutorControl = problem.OnWorkOrderExecutorControl,
                            InitiatorID = problem.InitiatorID,
                            InitiatorFullName = User.GetFullName(problem.InitiatorID),
                            QueueID = problem.QueueID,
                            QueueName = problem.Queue.Name,
                            ExecutorID = problem.ExecutorID,
                            ExecutorFullName = User.GetFullName(problem.ExecutorID),
                            ServiceName = problem.Service.Name,
                        };

            return query;
        }
    }
}
