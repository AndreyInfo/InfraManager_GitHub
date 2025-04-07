using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations.Events
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNegotiationEvents(this IServiceCollection services)
        {
            services
                .AddScoped<ISubjectFinder<NegotiationUser, Negotiation>, NegotiationUserSubjectFinder>() // можно и через ISelf... но для наглядности регистрирую тут
                .AddScoped<IConfigureEventWriter<NegotiationUser, Negotiation>, NegotiationUserEventConfigurer>()
                .AddScoped<NegotiationUserPropertyEventParamsConfigurer>()
                .WriteEventsOf<NegotiationUser, Negotiation>()
                    .WhenAddedIf(OperationID.Negotiation_AddUser, (state, entity) => string.IsNullOrWhiteSpace(entity.OldUserName))
                        .WithNegotiationUserMessage((nu, n, user) => $"[Пользователь] '{user.FullName}' добавлен к [согласованию] '{n.Name}'")
                        .AndSubjectBuilder<NegotiationUserSubjectBuilder>()
                    .WhenAddedIf(OperationID.Negotiation_Assign, (state, entity) => !string.IsNullOrWhiteSpace(entity.OldUserName))
                        .WithNegotiationUserMessage((nu, n, user) => $"[Пользователь] '{nu.OldUserName}' делегировал согласование пользователю '{user.FullName}'")
                        .AndSubjectBuilder<NegotiationUserSubjectBuilder>()
                    .WhenDeletedIf<NegotiationExists>(OperationID.Negotiation_RemoveUser)
                        .WithNegotiationUserMessage((nu, n, user) => $"[Пользователь] '{user.FullName}' исключен из согласования")
                        .AndSubjectBuilder<NegotiationUserSubjectBuilder>()
                    .WhenUpdatedIf(
                        OperationID.Negotiation_Vote, 
                        (state, user) => user.VotingType != (VotingType)state[nameof(NegotiationUser.VotingType)])
                        .WithNegotiationUserMessage((nu, n, user) => $"[Пользователь] '{user.FullName}' проголосовал")
                        .WithParamBuildersCollectionConfig<NegotiationUserPropertyEventParamsConfigurer>()
                        .AndSubjectBuilder<NegotiationUserSubjectBuilder>()
                    .WhenUpdatedIf(
                        OperationID.Negotiation_Comment,
                        (state, user) => user.Message != state[nameof(NegotiationUser.Message)]?.ToString())
                        .WithNegotiationUserMessage((nu, n, user) => $"[Пользователь] '{user.FullName}' оставил комментарий")
                        .WithParamBuildersCollectionConfig<NegotiationUserPropertyEventParamsConfigurer>()
                        .AndSubjectBuilder<NegotiationUserSubjectBuilder>()
                    .WhenUpdatedIf(
                        OperationID.Negotiation_AssignToAnotherNegotiator, 
                        (state, entity) => entity.OldUserName != state[nameof(NegotiationUser.OldUserName)]?.ToString())
                        .WithNegotiationUserMessage((nu, n, user) => $"[Пользователь] '{nu.OldUserName}' делегировал согласование пользователю '{user.FullName}'")    
                        .AndSubjectBuilder<NegotiationUserSubjectBuilder>()
                    .Submit();
            services
                .AddNegotiationEvents<CallNegotiation>()
                .AddNegotiationEvents<ProblemNegotiation>()
                .AddNegotiationEvents<WorkOrderNegotiation>()
                .AddNegotiationEvents<ChangeRequestNegotiation>()
                .AddNegotiationEvents<MassiveIncidentNegotiation>();

            return services;
        }

        private static EventOperationConfig<NegotiationUser, Negotiation> WithNegotiationUserMessage(
            this EventOperationConfig<NegotiationUser, Negotiation> config,
            Func<NegotiationUser, Negotiation, User, string> messageBuilder)
        {
            return config.WithMessageBuilder(
                serviceProvider => new CustomInjector().Inject(messageBuilder).GetService<NegotiationUserMessageBuilder>(serviceProvider));
        }

        private static IServiceCollection AddNegotiationEvents<TNegotiation>(this IServiceCollection services)
            where TNegotiation : Negotiation
        {
            services.AddScoped<IConfigureEventWriter<TNegotiation, TNegotiation>, NegotiationEventConfigurer<TNegotiation>>();
            services.AddScoped<NegotiationPropertyEventParamsConfigurer<TNegotiation>>();
            return services
                .WriteEventsOf<TNegotiation>()
                    .WhenAdded(OperationID.Negotiation_Insert)
                        .WithMessage(negotiation => $"Создано [Согласование] '{negotiation.Name}'")
                        .WithParamBuildersCollectionConfig<NegotiationPropertyEventParamsConfigurer<TNegotiation>>()
                        .AndSubjectBuilder<NegotiationSubjectBuilder<TNegotiation>>()
                    .WhenUpdated(OperationID.Negotiation_Update)
                        .WithMessage(negotiation => $"Изменено [Согласование] '{negotiation.Name}'")
                        .WithParamBuildersCollectionConfig<NegotiationPropertyEventParamsConfigurer<TNegotiation>>()
                        .AndSubjectBuilder<NegotiationSubjectBuilder<TNegotiation>>()
                    .WhenDeleted(OperationID.Negotiation_Delete)
                        .WithMessage(negotiation => $"Удалено [Согласование] '{negotiation.Name}'")
                        .AndSubjectBuilder<NegotiationSubjectBuilder<TNegotiation>>()
                    .Submit();
        }
    }
}
