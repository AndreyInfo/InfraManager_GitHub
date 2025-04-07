using AutoMapper;
using Inframanager;
using InfraManager.DAL.AutoMapper;
using InfraManager.DAL.Documents;
using System;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    internal sealed class WorkOrdersUnderControlMappingCreator :
        WorkOrderIssueMappingCreatorBase<ObjectUnderControlQueryResultItem>,
        ISelfRegisteredService<ICreateConfigurationProvider<WorkOrder, ObjectUnderControlQueryResultItem, Guid>>
    {
        private readonly NoteCountExpressionCreator<WorkOrder> _noteCountCreator;

        public WorkOrdersUnderControlMappingCreator(
            NoteCountExpressionCreator<WorkOrder> noteCountCreator,
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<WorkOrder> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
            _noteCountCreator = noteCountCreator;
        }

        protected override void ConfigureIssue(IMappingExpression<WorkOrder, ObjectUnderControlQueryResultItem> config, Guid userID)
        {
            config
                .WithClientUnderControl(workOrder => workOrder.Initiator)
                .With(
                    item => item.CanBePicked,
                    workOrder => workOrder.QueueID != null
                        && workOrder.Group.QueueUsers.Any(u => u.UserID == userID)
                        && workOrder.ExecutorID == null)
                .WithNoteAndMessageCounts(_noteCountCreator);
        }
    }
}


