using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    internal class NegotiationProblemListSubQuery : IListQuery<Problem, NegotiationListSubQueryResultItem>, ISelfRegisteredService<IListQuery<Problem, NegotiationListSubQueryResultItem>>
    {
        private readonly DbContext _db;

        public NegotiationProblemListSubQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<NegotiationListSubQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<Problem, bool>>> filterBy)
        {
            IQueryable<Problem> problems = _db.Set<Problem>().AsNoTracking();

            foreach (var filterExpression in filterBy)
            {
                problems = problems.Where(filterExpression);
            }

                return  from problem in problems
                        let messagesCount = _db.Set<Note<Problem>>()
                            .Count(n => n.ParentObjectID == problem.IMObjID && n.Type == SDNoteType.Message)
                        let notesCount = _db.Set<Note<Problem>>()
                         .Count(n => n.ParentObjectID == problem.IMObjID && n.Type == SDNoteType.Note)
                        select new NegotiationListSubQueryResultItem
                        {
                            ObjectID = problem.IMObjID,              
                            Number = problem.Number,                   
                            ClassID = ObjectClass.Problem,
                            CategorySortColumn = Issues.Problem,
                            Name = DbFunctions.CastAsString(problem.Summary),                 
                            WorkflowSchemeIdentifier = problem.WorkflowSchemeIdentifier,
                            WorkflowSchemeVersion = problem.WorkflowSchemeVersion,
                            EntityStateName = problem.EntityStateName, 
                            EntityStateID = problem.EntityStateID,
                            WorkflowSchemeID = problem.WorkflowSchemeID,
                            PriorityName = problem.Priority.Name,
                            PriorityColor = problem.Priority.Color,
                            PriorityID = problem.Priority.ID,
                            PrioritySequence = problem.Priority.Sequence,
                            TypeFullName = DbFunctions.CastAsString(ProblemType.GetFullProblemTypeName(problem.Type.ID)),                  
                            TypeID = problem.Type.ID,
                            OwnerID = problem.OwnerID,
                            ExecutorID = problem.ExecutorID,
                            QueueID = problem.QueueID,
                            UtcDateRegistered = problem.UtcDateDetected,
                            UtcDateModified = problem.UtcDateModified,
                            UtcDateClosed = problem.UtcDateClosed,
                            UtcDatePromised = DbFunctions.CastAsDateTime(problem.UtcDatePromised),
                            UtcDateAccomplished = problem.UtcDateSolved,
                            ClientID = null,
                            ClientSubdivisionID = null,
                            ClientOrganizationID = null,
                            ClientFullName = null,
                            ClientSubdivisionFullName = null,
                            ClientOrganizationName = null,
                            CanBePicked = false,
                            IsFinished = problem.UtcDateSolved != null,
                            NoteCount = notesCount,
                            MessagesCount = messagesCount
                        };

        }
    }
}


