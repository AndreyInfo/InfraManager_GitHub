using AutoMapper;
using Inframanager;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal abstract class MassIncidentIssueMappingCreatorBase<TIssue> : IssueMappingConfigurationCreatorBase<MassIncident, TIssue>
        where TIssue : IssueQueryResultItem
    {
        protected MassIncidentIssueMappingCreatorBase(
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<MassIncident> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
        }

        protected override void ConfigureEntity(IMappingExpression<MassIncident, TIssue> mapping, Guid userID)
        {
            mapping
                .WithCategory(Issues.MassIncident)
                .With(item => item.Number, massIncident => massIncident.ID)
                .WithCast(item => item.Name, massIncident => massIncident.Name)
                .With(item => item.TypeID, massIncident => massIncident.Type.IMObjID)
                .WithCast(item => item.TypeFullName, massIncident => massIncident.Type.Name)
                .With(item => item.OwnerID, massIncident => massIncident.OwnedBy.IMObjID)
                .With(item => item.OwnerFullName, MassIncident.OwnerFullName)
                .With(item => item.ExecutorID, massIncident => massIncident.ExecutedByUser.IMObjID)
                .WithCast(item => item.ExecutorFullName, MassIncident.ExecutorFullName)
                .With(item => item.QueueID, massIncident => massIncident.ExecutedByGroupID)
                .WithGroupName(massIncident => massIncident.ExecutedByGroup)
                .With(item => item.UtcDatePromised, massIncident => massIncident.UtcCloseUntil)
                .With(item => item.UtcDateRegistered, massIncident => massIncident.UtcRegisteredAt)
                .WithClientCommon(massIncident => massIncident.CreatedBy)
                .With(item => item.IsFinished, massIncident => massIncident.UtcDateAccomplished != null)
                .With(item => item.IsOverdue, massIncident => massIncident.UtcCloseUntil != null && massIncident.UtcCloseUntil < DateTime.UtcNow);
        }
    }
}
