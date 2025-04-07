using Inframanager.BLL.Events;
using InfraManager.BLL.ServiceDesk.Notes;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests.Events
{
    internal static class ServiceCollectionExtensions
    {
        private const string Subject = "Запрос на изменения";

        public static IServiceCollection AddChangeRequestEvents(this IServiceCollection services)
        {
            return services
                .AddScoped<ChangeRequestEventParamsConfigurer>()
                .WriteEventsOf<ChangeRequest>()
                    .WhenAdded(OperationID.ChangeRequest_Add)
                        .WithMessage(x => $"Создан [{Subject}] '{x.Number}'")
                        .WithParamBuildersCollectionConfig<ChangeRequestEventParamsConfigurer>()
                        .AndSubjectBuilder<ChangeRequestEventSubjectBuilder>()
                    .WhenUpdated(OperationID.ChangeRequest_Update)
                        .WithMessage(x => $"Сохранен [{Subject}] '{x.Number}'")
                        .WithParamBuildersCollectionConfig<ChangeRequestEventParamsConfigurer>()
                        .AndSubjectBuilder<ChangeRequestEventSubjectBuilder>()
                    .WhenDeleted(OperationID.ChangeRequest_Delete)
                        .WithMessage(x => $"Удален [{Subject}] '{x.Number}'")
                        .AndSubjectBuilder<ChangeRequestEventSubjectBuilder>()
                .Submit()
                .AddScoped<IConfigureEventWriter<CallReference<ChangeRequest>, ChangeRequest>, CallReferenceEventWriterConfigurer>()
                .AddScoped<ISubjectFinder<CallReference<ChangeRequest>, ChangeRequest>, CallReferenceSubjectFinder<ChangeRequest>>()
                .WriteEventsOf<CallReference<ChangeRequest>, ChangeRequest>()
                    .WhenAdded(OperationID.CallReference_Add)
                        .WithMessageBuilder(CallReferenceMessageBuilder("Добавлена"))
                        .AndSubjectBuilder<CallReferenceEventSubjectBuilder>()
                    .WhenDeleted(OperationID.CallReference_Remove)
                        .WithMessageBuilder(CallReferenceMessageBuilder("Удалена"))
                        .AndSubjectBuilder<CallReferenceEventSubjectBuilder>()
                .Submit()
                .AddNoteEvents<ChangeRequest>();
        }

        private static Func<IServiceProvider, IBuildEventMessage<CallReference<ChangeRequest>, ChangeRequest>> CallReferenceMessageBuilder(string action)
        {
            return serviceProvider => new CustomInjector()
                .Inject(action)
                .GetService<CallReferenceEventMessageBuilder<ChangeRequest>>(serviceProvider);
        }

        private class ChangeRequestEventSubjectBuilder : ServiceDeskEntityEventSubjectBuilder<ChangeRequest>
        {
            public ChangeRequestEventSubjectBuilder() : base(Subject)
            {
            }
        }

        private class CallReferenceEventSubjectBuilder : EventSubjectBuilderBase<CallReference<ChangeRequest>, ChangeRequest>
        {
            public CallReferenceEventSubjectBuilder() : base(Subject)
            {
            }

            protected override Guid GetID(ChangeRequest subject)
            {
                return subject.IMObjID;
            }

            protected override string GetSubjectValue(ChangeRequest subject)
            {
                return subject.Summary;
            }
        }
    }
}
