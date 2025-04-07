using AutoMapper;
using Inframanager;
using InfraManager.DAL.AutoMapper;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal sealed class MyTasksMassIncidentMappingCreator :
        MassIncidentIssueMappingCreatorBase<MyTasksListQueryResultItem>,
        ISelfRegisteredService<ICreateConfigurationProvider<MassIncident, MyTasksListQueryResultItem, Guid>>
    {
        public MyTasksMassIncidentMappingCreator(
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<MassIncident> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
        }

        protected override void ConfigureIssue(IMappingExpression<MassIncident, MyTasksListQueryResultItem> mapping, Guid userID)
        {
        }
    }
}
