using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk.Manhours;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfraManager.BLL.ServiceDesk.Manhours.Events
{
    internal static class ServiceCollectionExtensions
    {
        private const string Subject = "Работа";

        private static Func<ManhoursWork, string> EventMessage(string action) =>
            x => $"{action} [{Subject}] '№{x.Number}'";

        private static Func<ManhoursWork, string> EventSavedMessage => EventMessage("Сохранена");

        public static IServiceCollection AddManhoursEvents(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureEventWriter<ManhoursWork, ManhoursWork>, WorkEventWriterConfigurer>()
                .AddScoped<WorkEventParamsConfigurer>()
                .WriteEventsOf<ManhoursWork>()
                    .WhenAdded(OperationID.ManhoursWork_Add)
                        .WithMessage(EventMessage("Добавлена"))
                        .WithParamBuildersCollectionConfig<WorkEventParamsConfigurer>()
                        .AndSubjectBuilder<EventSubjectBuilder>()
                    .WhenUpdated(OperationID.ManhoursWork_Update)
                        .WithMessage(EventSavedMessage)
                        .WithParamBuildersCollectionConfig<WorkEventParamsConfigurer>()
                        .AndSubjectBuilder<EventSubjectBuilder>()
                    .WhenDeleted(OperationID.ManhoursWork_Delete)
                        .WithMessage(EventMessage("Удалена"))
                        .AndSubjectBuilder<EventSubjectBuilder>()
                .Submit()
                .AddTransient<IConfigureEventWriter<ManhoursEntry, ManhoursWork>, EntryEventWriterConfigurer>()
                .AddScoped<ISubjectFinder<ManhoursEntry, ManhoursWork>, EntrySubjectFinder>()
                .WriteEventsOf<ManhoursEntry, ManhoursWork>()
                    .WhenAdded(OperationID.ManhoursEntry_Add)
                        .WithMessage((entry, subj) => EventSavedMessage(subj))
                        .WithParamBuildersCollection(
                            serviceProvider => new CustomInjector()
                                .Inject("Трудозатрата добавлена")
                                .GetService<EntryEventParamsCollection>(serviceProvider))
                        .AndSubjectBuilder<EventSubjectBuilder>()
                    .WhenUpdated(OperationID.ManhoursEntry_Update)
                        .WithMessage((entry, subj) => EventSavedMessage(subj))
                        .WithParamBuildersCollection(
                            serviceProvider => new CustomInjector()
                                .Inject("Трудозатрата обновлена")
                                .GetService<EntryEventParamsCollection>(serviceProvider))
                        .AndSubjectBuilder<EventSubjectBuilder>()
                    .WhenDeleted(OperationID.ManhoursEntry_Remove)
                        .WithMessage((entry, subj) => EventSavedMessage(subj))
                        .WithParamBuildersCollection(
                            serviceProvider => new CustomInjector()
                                .Inject("Трудозатрата удалена")
                                .GetService<EntryEventParamsCollection>(serviceProvider))
                        .AndSubjectBuilder<EventSubjectBuilder>()
                .Submit();
        }
    }
}
