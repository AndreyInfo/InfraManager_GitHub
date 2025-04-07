using AutoMapper;
using Inframanager;
using InfraManager.DAL.AutoMapper;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal sealed class MyTasksCallMappingCreator :
        CallIssueMappingCreatorBase<MyTasksListQueryResultItem>,
        ISelfRegisteredService<ICreateConfigurationProvider<Call, MyTasksListQueryResultItem, Guid>>
    {
        public MyTasksCallMappingCreator(
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<Call> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
        }

        protected override void ConfigureIssue(IMappingExpression<Call, MyTasksListQueryResultItem> mapping, Guid userID)
        {
        }
    }
}
