using AutoMapper;
using Inframanager;
using InfraManager.DAL.AutoMapper;
using InfraManager.DAL.Documents;
using System;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal sealed class ProblemsUnderControlMappingCreator :
        ProblemIssueMappingCreatorBase<ObjectUnderControlQueryResultItem>,
        ISelfRegisteredService<ICreateConfigurationProvider<Problem, ObjectUnderControlQueryResultItem, Guid>>
    {
        private readonly NoteCountExpressionCreator<Problem> _noteCountCreator;

        public ProblemsUnderControlMappingCreator(
            NoteCountExpressionCreator<Problem> noteCountCreator,
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<Problem> objectClassProvider) 
            : base(unreadMessageCountCreator, documentCountCreator, objectClassProvider)
        {
            _noteCountCreator = noteCountCreator;
        }

        protected override void ConfigureIssue(IMappingExpression<Problem, ObjectUnderControlQueryResultItem> config, Guid userID)
        {
            config
                .WithNull(item => item.ClientSubdivisionID)
                .WithNull(item => item.ClientOrganizationID)
                .With(item => item.CanBePicked, problem => false)
                .WithNoteAndMessageCounts(_noteCountCreator);
        }
    }
}


