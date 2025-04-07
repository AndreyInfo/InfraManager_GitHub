using AutoMapper;
using Inframanager;
using InfraManager.DAL.AutoMapper;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal sealed class MyTasksProblemMappingCreator :
        ProblemIssueMappingCreatorBase<MyTasksListQueryResultItem>,
        ISelfRegisteredService<ICreateConfigurationProvider<Problem, MyTasksListQueryResultItem, Guid>>
    {
        public MyTasksProblemMappingCreator(
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<Problem> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
        }

        protected override void ConfigureIssue(IMappingExpression<Problem, MyTasksListQueryResultItem> mapping, Guid userID)
        {
        }
    }
}
