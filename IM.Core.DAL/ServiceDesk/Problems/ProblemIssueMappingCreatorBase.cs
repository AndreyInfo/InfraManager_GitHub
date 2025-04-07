using AutoMapper;
using Inframanager;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal abstract class ProblemIssueMappingCreatorBase<TIssue> : IssueMappingConfigurationCreatorBase<Problem, TIssue>
        where TIssue : IssueQueryResultItem
    {
        protected ProblemIssueMappingCreatorBase(
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<Problem> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
        }

        protected override void ConfigureEntity(IMappingExpression<Problem, TIssue> mapping, Guid userID)
        {
            mapping
                .WithCategory(Issues.Problem)
                .WithCast(item => item.Name, problem => problem.Summary)
                .WithCast(item => item.TypeFullName, problem => ProblemType.GetFullProblemTypeName(problem.Type.ID))
                .With(item => item.OwnerFullName, Problem.OwnerFullName)
                .WithCast(item => item.ExecutorFullName, Problem.ExecutorFullName)
                .WithGroupName(problem => problem.Queue)
                .With(item => item.UtcDateRegistered, problem => problem.UtcDateDetected)
                .With(item => item.UtcDateAccomplished, problem => problem.UtcDateSolved)
                .WithClientCommon(problem => problem.Initiator)
                .With(item => item.IsFinished, problem => problem.UtcDateSolved != null)
                .With(item => item.IsOverdue, problem => problem.UtcDatePromised < DateTime.UtcNow);
        }
    }
}
