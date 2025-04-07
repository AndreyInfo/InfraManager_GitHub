using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKnowledgeBaseArticleEvents(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureEventWriter<KBArticleReference, KBArticleReference>, ReferenceEventWriterConfigurer>()
                .WriteEventsOf<KBArticleReference, KBArticleReference>()
                    .WhenAdded(OperationID.KnowledgeBaseArticleReference_Add)
                        .WithMessageBuilder(MessageBuildOfAction("добавлена"))
                        .AndSubjectBuilder<ArticleReferenceEventSubjectBuilder>()
                    .WhenDeleted(OperationID.KnowledgeBaseArticleReference_Remove)
                        .WithMessageBuilder(MessageBuildOfAction("удалена"))
                        .AndSubjectBuilder<ArticleReferenceEventSubjectBuilder>()
                .Submit();
        }

        private static Func<IServiceProvider, IBuildEventMessage<KBArticleReference, KBArticleReference>> MessageBuildOfAction(string actionName) =>
            provider => new CustomInjector().Inject(actionName).GetService<ArticleReferenceEventMessageBuilder>(provider);
    }
}
