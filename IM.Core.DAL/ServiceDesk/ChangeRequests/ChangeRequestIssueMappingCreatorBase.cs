using AutoMapper;
using Inframanager;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal abstract class ChangeRequestIssueMappingCreatorBase<TIssue> : IssueMappingConfigurationCreatorBase<ChangeRequest, TIssue>
        where TIssue : IssueQueryResultItem
    {
        protected ChangeRequestIssueMappingCreatorBase(
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<ChangeRequest> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
        }

        protected override void ConfigureEntity(IMappingExpression<ChangeRequest, TIssue> mapping, Guid userID)
        {
            mapping
                .WithCategory(Issues.ChangeRequest)
                .WithCast(item => item.Name, rfc => rfc.Summary)
                .With(item => item.TypeID, rfc => rfc.RFCTypeID)
                .WithCast(item => item.TypeFullName, rfc => rfc.Type.Name)
                .With(item => item.OwnerFullName, ChangeRequest.OwnerFullName)
                .WithNull(item => item.ExecutorID)
                .WithCast(item => item.ExecutorFullName, rfc => null)
                .WithNull(item => item.QueueID)
                .WithNull(item => item.QueueName)
                .With(item => item.UtcDateRegistered, rfc => rfc.UtcDateDetected)
                .With(item => item.UtcDateAccomplished, rfc => rfc.UtcDateSolved)
                .WithNull(item => item.ClientID)
                .WithCast(item => item.ClientFullName, rfc => null)
                .WithNull(item => item.ClientSubdivisionFullName)
                .WithCast(item => item.ClientOrganizationName, rfc => null)
                .With(item => item.IsFinished, rfc => rfc.UtcDateSolved != null)
                .With(item => item.IsOverdue, rfc => rfc.UtcDatePromised != null && rfc.UtcDatePromised < DateTime.UtcNow);
        }
    }
}
