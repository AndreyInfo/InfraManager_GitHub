using AutoMapper;
using Inframanager;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal abstract class CallIssueMappingCreatorBase<TIssue> : IssueMappingConfigurationCreatorBase<Call, TIssue>
        where TIssue : IssueQueryResultItem
    {
        protected CallIssueMappingCreatorBase(
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<Call> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
        }

        protected override void ConfigureEntity(IMappingExpression<Call, TIssue> mapping, Guid userID)
        {
            mapping
                .WithCategory(Issues.Call)
                .WithCast(item => item.Name, call => call.CallSummaryName)
                .With(item => item.TypeID, call => call.CallTypeID)
                .WithCast(item => item.TypeFullName, call => CallType.GetFullCallTypeName(call.CallType.ID))
                .With(item => item.OwnerFullName, Call.OwnerFullName)
                .With(item => item.ExecutorID, call => call.ExecutorID)
                .WithCast(item => item.ExecutorFullName, Call.ExecutorFullName)
                .WithClient(call => call.Client)
                .With(item => item.ClientSubdivisionFullName, Call.ClientSubdivisionFullName)
                .WithCast(item => item.ClientOrganizationName, call => call.ClientSubdivision.Organization.Name)
                .With(item => item.QueueID, call => call.QueueID)
                .WithGroupName(call => call.Queue)
                .With(item => item.IsOverdue, Call.IsOverdue);
        }
    }
}
