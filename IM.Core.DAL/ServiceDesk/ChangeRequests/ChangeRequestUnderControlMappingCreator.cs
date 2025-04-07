using AutoMapper;
using Inframanager;
using InfraManager.DAL.AutoMapper;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal sealed class ChangeRequestUnderControlMappingCreator :
        ChangeRequestIssueMappingCreatorBase<ObjectUnderControlQueryResultItem>,
        ISelfRegisteredService<ICreateConfigurationProvider<ChangeRequest, ObjectUnderControlQueryResultItem, Guid>>
    {
        private readonly NoteCountExpressionCreator<ChangeRequest> _noteCountCreator;

        public ChangeRequestUnderControlMappingCreator(
            NoteCountExpressionCreator<ChangeRequest> noteCountCreator,
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<ChangeRequest> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
            _noteCountCreator = noteCountCreator;
        }

        protected override void ConfigureIssue(IMappingExpression<ChangeRequest, ObjectUnderControlQueryResultItem> config, Guid userID)
        {
            config
                .WithNull(item => item.ClientSubdivisionID)
                .WithNull(item => item.ClientOrganizationID)
                .With(item => item.CanBePicked, rfc => false)
                .WithNoteAndMessageCounts(_noteCountCreator);
        }
    }
}
