using IM.Core.Import.BLL.Interface.Ldap.Import;
using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.ProductCatalogue.Units;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using IM.Core.Schedule.BLL.Jobs;
using IM.Core.Schedule.BLL.Jobs.ImportJobs;
using IM.Core.Schedule.BLL.Jobs.ScheduleCalculators;
using IM.Core.ScheduleBLL.Interfaces;
using Inframanager.BLL;
using InfraManager.Services.ScheduleService;

namespace IM.Core.Schedule.BLL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScheduleBLL(this IServiceCollection services)
    {
        var thisAssembly = Assembly.GetExecutingAssembly();
        var interfaceAssembly = typeof(IScheduleBLL).Assembly;
        services.AddSelfRegisteredServices(thisAssembly);
        
        services.AddMemoryCache();
        
        services.AddBLLCore(thisAssembly, interfaceAssembly);


        services.AddMappingScoped(
            new ServiceMapping<TaskTypeEnum, IBaseJob>()
                .Map<UserImportJob>().To(TaskTypeEnum.UsersImport)
                .Map<ProductCatalogImportJob>().To(TaskTypeEnum.ProductCatalogImport)
                .Map<ITAssetImport>().To(TaskTypeEnum.ITAssetImport));

        services.AddMappingScoped(
            new ServiceMapping<ScheduleTypeEnum, BaseScheduleCalculator>()
                .Map<ImmediatelyScheduleCalculator>().To(ScheduleTypeEnum.Immediately)
                .Map<ImmediatelyScheduleCalculator>().To(ScheduleTypeEnum.Once)
                .Map<DailyScheduleCalculator>().To(ScheduleTypeEnum.Daily)
                .Map<WeeklyScheduleCalculator>().To(ScheduleTypeEnum.Weekly)
                .Map<MonthlyScheduleCalculator>().To(ScheduleTypeEnum.Monthly));

        return services;
    }
}