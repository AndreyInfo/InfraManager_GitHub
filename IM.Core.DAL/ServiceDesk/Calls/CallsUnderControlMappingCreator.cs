using AutoMapper;
using Inframanager;
using InfraManager.DAL.AutoMapper;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal sealed class CallsUnderControlMappingCreator : CallIssueMappingCreatorBase<ObjectUnderControlQueryResultItem>,
        ISelfRegisteredService<ICreateConfigurationProvider<Call, ObjectUnderControlQueryResultItem, Guid>>
    {
        private readonly NoteCountExpressionCreator<Call> _noteCountCreator;

        public CallsUnderControlMappingCreator(
            NoteCountExpressionCreator<Call> noteCountCreator,
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<Call> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
            _noteCountCreator = noteCountCreator;
        }

        protected override void ConfigureIssue(IMappingExpression<Call, ObjectUnderControlQueryResultItem> config, Guid userID)
        {
            config
                .With(item => item.ClientOrganizationID, call => call.ClientSubdivision.OrganizationID)
                .With(item => item.CanBePicked, Call.CanBePicked(userID))
                .WithNoteAndMessageCounts(_noteCountCreator);
        }
    }
}
