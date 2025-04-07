using Inframanager.BLL.Events;
using InfraManager.BLL.ServiceDesk.Notes;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents.Events
{
    internal static class ServiceCollectionExtensions
    {
        private static string Subject = "Массовый инцидент";

        public static IServiceCollection AddMassIncidentEvents(this IServiceCollection services)
        {
            return services
                .AddScoped<MassIncidentPropertyBuildersCollectionConfigurer>()
                .AddTransient<IConfigureEventWriter<MassIncident, MassIncident>, MassIncidentEventWriterConfigurer>()
                .WriteEventsOf<MassIncident>()
                    .WhenAdded(OperationID.MassIncident_Add)
                        .WithMessage(massIncident => $"Создан [{Subject}] '{massIncident.ID}'")
                        .WithParamBuildersCollectionConfig<MassIncidentPropertyBuildersCollectionConfigurer>()
                        .AndSubjectBuilder<MassIncidentEventSubjectBuilder>()
                    .WhenUpdated(OperationID.MassIncident_Update)
                        .WithMessage(massIncident => $"Сохранен [{Subject}] '{massIncident.ID}'")
                        .WithParamBuildersCollectionConfig<MassIncidentPropertyBuildersCollectionConfigurer>()
                        .AndSubjectBuilder<MassIncidentEventSubjectBuilder>()
                    .WhenDeleted(OperationID.MassIncident_Delete)
                        .WithMessage(massIncident => $"Удален [{Subject}] '{massIncident.ID}'")
                        .AndSubjectBuilder<MassIncidentEventSubjectBuilder>()
                 .Submit()
                 .AddNoteEvents<MassIncident>()
                 .AddScoped<ISubjectFinder<ManyToMany<MassIncident, Service>, MassIncident>, AffectedServiceSubjectFinder>()
                 .AddTransient<IConfigureEventWriter<ManyToMany<MassIncident, Service>, MassIncident>, AffectedServiceEventWriterConfigurer>()
                 .WriteEventsOf<ManyToMany<MassIncident, Service>, MassIncident>()
                    .WhenAdded(OperationID.MassIncidentAffectedService_Add)
                        .WithMessage(entity => $"Добавлен доп. сервис \"{entity.Reference.Name}\"")
                        .AndSubjectBuilder<MassIncidentAffectedServiceEventSubjectBuilder>()
                    .WhenDeleted(OperationID.MassIncidentAffectedService_Delete)
                        .WithMessage(entity => $"Удален доп. сервис \"{entity.Reference.Name}\"")
                        .AndSubjectBuilder<MassIncidentAffectedServiceEventSubjectBuilder>()
                 .Submit();
        }

        private class MassIncidentEventSubjectBuilder : EventSubjectBuilderBase<MassIncident, MassIncident>
        {
            public MassIncidentEventSubjectBuilder() : base(Subject)
            {
            }

            protected override Guid GetID(MassIncident subject)
            {
                return subject.IMObjID;
            }

            protected override string GetSubjectValue(MassIncident subject)
            {
                return subject.ID.ToString();
            }
        }

        private class MassIncidentAffectedServiceEventSubjectBuilder : 
            EventSubjectBuilderBase<ManyToMany<MassIncident, Service>, MassIncident>
        {
            public MassIncidentAffectedServiceEventSubjectBuilder() : base(Subject)
            {
            }

            protected override Guid GetID(MassIncident subject)
            {
                return subject.IMObjID;
            }

            protected override string GetSubjectValue(MassIncident subject)
            {
                return subject.ID.ToString();
            }
        }
    }
}
