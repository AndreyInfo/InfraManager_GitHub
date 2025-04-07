using AutoMapper;
using Inframanager;
using InfraManager.DAL.AutoMapper;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    internal sealed class MyTasksWorkOrderMappingCreator :
        WorkOrderIssueMappingCreatorBase<MyTasksListQueryResultItem>,
        ISelfRegisteredService<ICreateConfigurationProvider<WorkOrder, MyTasksListQueryResultItem, Guid>>
    {
        public MyTasksWorkOrderMappingCreator(
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<WorkOrder> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
        }

        protected override void ConfigureIssue(IMappingExpression<WorkOrder, MyTasksListQueryResultItem> mapping, Guid userID)
        {
        }
    }
}
