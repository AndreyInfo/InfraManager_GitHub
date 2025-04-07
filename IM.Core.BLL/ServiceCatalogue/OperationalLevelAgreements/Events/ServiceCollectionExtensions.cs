using System;
using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements.Events;

public static class ServiceCollectionExtensions
{
    private static string Subject = "Соглашение об уровне обслуживания";

    public static IServiceCollection AddOLAEvents(this IServiceCollection services)
    {
        return services
            .AddScoped<OperationalLevelAgreementPropertyBuildersCollectionConfigurer>()
            .AddTransient<IConfigureEventWriter<OperationalLevelAgreement, OperationalLevelAgreement>,
                OperationalLevelAgreementEventWriterConfigurer>()
            .WriteEventsOf<OperationalLevelAgreement>()
                .WhenAdded(OperationID.OperationalLevelAgreement_Add)
                    .WithMessage(ola => $"Создан [{Subject}] '{ola.ID}'")
                    .WithParamBuildersCollectionConfig<OperationalLevelAgreementPropertyBuildersCollectionConfigurer>()
                    .AndSubjectBuilder<OperationalLevelAgreementEventSubjectBuilder>()
            .WhenUpdated(OperationID.OperationalLevelAgreement_Update)
                .WithMessage(ola => $"Сохранен [{Subject}] '{ola.ID}'")
                    .WithParamBuildersCollectionConfig<OperationalLevelAgreementPropertyBuildersCollectionConfigurer>()
                    .AndSubjectBuilder<OperationalLevelAgreementEventSubjectBuilder>()
            .WhenDeleted(OperationID.OperationalLevelAgreement_Delete)
                .WithMessage(ola => $"Удален [{Subject}] '{ola.ID}'")
                    .AndSubjectBuilder<OperationalLevelAgreementEventSubjectBuilder>()
            .Submit();
    }

    private class OperationalLevelAgreementEventSubjectBuilder : EventSubjectBuilderBase<OperationalLevelAgreement, OperationalLevelAgreement>
    {
        public OperationalLevelAgreementEventSubjectBuilder() : base(Subject)
        {
        }

        protected override Guid GetID(OperationalLevelAgreement subject)
        {
            return subject.IMObjID;
        }

        protected override string GetSubjectValue(OperationalLevelAgreement subject)
        {
            return subject.ID.ToString();
        }
    }
}