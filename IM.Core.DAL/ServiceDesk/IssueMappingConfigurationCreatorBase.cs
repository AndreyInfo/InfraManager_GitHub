using AutoMapper;
using InfraManager.DAL.Documents;
using Inframanager;
using System;
using InfraManager.DAL.AutoMapper;

namespace InfraManager.DAL.ServiceDesk
{
    internal abstract class IssueMappingConfigurationCreatorBase<TEntity, TIssue> : ICreateConfigurationProvider<TEntity, TIssue, Guid>
        where TEntity : IWorkflowEntity
        where TIssue : IssueQueryResultItem
    {
        private readonly UnreadMessageCountExpressionCreator _unreadMessageCountCreator;
        private readonly DocumentCountExpressionCreator _documentCountCreator;
        private readonly IObjectClassProvider<TEntity> _objectClassProvider;

        public IssueMappingConfigurationCreatorBase(
            UnreadMessageCountExpressionCreator unreadMessageCountCreator, 
            DocumentCountExpressionCreator documentCountCreator, 
            IObjectClassProvider<TEntity> objectClassProvider)
        {
            _unreadMessageCountCreator = unreadMessageCountCreator;
            _documentCountCreator = documentCountCreator;
            _objectClassProvider = objectClassProvider;
        }

        public IConfigurationProvider Create(Guid userID)
        {
            var classID = _objectClassProvider.GetObjectClass();
            return new MapperConfiguration(
                x =>
                {
                    var mapping = x.CreateMap<TEntity, TIssue>()
                        .ForMember(item => item.ID, mapper => mapper.MapFrom(entity => entity.IMObjID))
                        .ForMember(item => item.ClassID, mapper => mapper.MapFrom(entity => classID))
                        .ForMember(
                            item => item.UnreadMessageCount,
                            mapper => mapper.MapFrom(_unreadMessageCountCreator.Create<TEntity>(userID)))
                        .ForMember(
                            item => item.DocumentCount,
                            mapper => mapper.MapFrom(_documentCountCreator.Create<TEntity>()));
                    ConfigureEntity(mapping, userID);
                    ConfigureIssue(mapping, userID);
                });
        }

        protected abstract void ConfigureEntity(IMappingExpression<TEntity, TIssue> mapping, Guid userID);
        protected abstract void ConfigureIssue(IMappingExpression<TEntity, TIssue> mapping, Guid userID);
    }
}
