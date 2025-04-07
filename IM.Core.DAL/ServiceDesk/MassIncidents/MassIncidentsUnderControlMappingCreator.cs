using AutoMapper;
using Inframanager;
using InfraManager.DAL.AutoMapper;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal sealed class MassIncidentsUnderControlMappingCreator : 
        MassIncidentIssueMappingCreatorBase<ObjectUnderControlQueryResultItem>,
        ISelfRegisteredService<ICreateConfigurationProvider<MassIncident, ObjectUnderControlQueryResultItem, Guid>>
    {
        private readonly NoteCountExpressionCreator<MassIncident> _noteCountExpressionCreator;

        public MassIncidentsUnderControlMappingCreator(
            NoteCountExpressionCreator<MassIncident> noteCountExpressionCreator,
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<MassIncident> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
            _noteCountExpressionCreator = noteCountExpressionCreator;
        }

        protected override void ConfigureIssue(IMappingExpression<MassIncident, ObjectUnderControlQueryResultItem> config, Guid userID)
        {
            config
                .WithCategory(Issues.MassIncident)
                .WithClientUnderControl(massIncident => massIncident.CreatedBy)
                .With(item => item.CanBePicked, massIncident => false)
                .WithNoteAndMessageCounts(_noteCountExpressionCreator);  
        }
    }
}
