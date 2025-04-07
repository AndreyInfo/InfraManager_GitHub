using Inframanager.BLL.Events;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.ELP
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddELPEvents(this IServiceCollection services)
        {
            return services
                .AddScoped<IConfigureEventWriter<ElpSetting, ElpSetting>, ELPSettingEventConfigurer>()
                .AddScoped<ELPSettingSubjectBuilder>()
                .AddScoped<ELPSettingPropertyParamsConfigurer>()
                .WriteEventsOf<ElpSetting>()
                    .WhenAdded(OperationID.ELPSetting_Insert)
                        .WithMessage(elp => $"Создана [Связь между инсталляциями и лицензиями] '{elp.Name}'")
                        .WithParamBuildersCollectionConfig<ELPSettingPropertyParamsConfigurer>()
                        .AndSubjectBuilder<ELPSettingSubjectBuilder>()
                    .WhenUpdated(OperationID.ELPSetting_Update)
                        .WithMessage(elp => $"Изменена [Связь между инсталляциями и лицензиями] '{elp.Name}'")
                        .WithParamBuildersCollectionConfig<ELPSettingPropertyParamsConfigurer>()
                        .AndSubjectBuilder<ELPSettingSubjectBuilder>()
                    .WhenDeleted(OperationID.ELPSetting_Delete)
                        .WithMessage(elp => $"Удалена [Связь между инсталляциями и лицензиями] '{elp.Name}'")
                        .AndSubjectBuilder<ELPSettingSubjectBuilder>()
                .Submit();
        }
    }
}