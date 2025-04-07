using AutoMapper;
using Inframanager;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    internal abstract class WorkOrderIssueMappingCreatorBase<TIssue> : IssueMappingConfigurationCreatorBase<WorkOrder, TIssue>
        where TIssue : IssueQueryResultItem
    {
        protected WorkOrderIssueMappingCreatorBase(
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<WorkOrder> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
        }

        protected override void ConfigureEntity(IMappingExpression<WorkOrder, TIssue> mapping, Guid userID)
        {
            mapping
                .WithCategory(Issues.WorkOrder)
                .WithCast(item => item.Name, workOrder => workOrder.Name)
                .WithCast(item => item.TypeFullName, workOrder => workOrder.Type.Name)
                .With(item => item.OwnerID, workOrder => workOrder.AssigneeID)
                .With(item => item.OwnerFullName, WorkOrder.AssigneeFullName)
                .With(item => item.ExecutorID, workOrder => workOrder.ExecutorID)
                .WithCast(item => item.ExecutorFullName, WorkOrder.ExecutorFullName)
                .WithGroupName(item => item.Group)
                .With(item => item.UtcDateRegistered, workOrder => workOrder.UtcDateAssigned)
                .With(item => item.UtcDateModified, workOrder => workOrder.UtcDateModified)
                .With(item => item.UtcDateClosed, workOrder => workOrder.UtcDateAccomplished)
                .WithClientCommon(workOrder => workOrder.Initiator)
                .With(item => item.IsFinished, workOrder => workOrder.UtcDateAccomplished != null)
                .With(item => item.IsOverdue, workOrder => workOrder.UtcDatePromised < DateTime.UtcNow);
        }
    }
}
