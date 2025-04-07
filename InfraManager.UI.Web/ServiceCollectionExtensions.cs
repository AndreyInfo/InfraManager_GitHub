using System;
using System.IO;
using IM.Core.BLL.Database;
using IM.Core.DAL.PGSQL;
using InfraManager.BLL;
using InfraManager.BLL.Import;
using InfraManager.BLL.Workflow;
using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.DAL;
using InfraManager.DAL.Microsoft.SqlServer;
using InfraManager.DemoChecker;
using InfraManager.DependencyInjection;
using InfraManager.ServiceBase.MailService;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.ServiceBase.SearchService;
using InfraManager.UI.Web.Services;
using InfraManager.WebAPIClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace InfraManager.UI.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IWorkflowServiceApi>(
                provider => new WorkflowServiceClient(
                    configuration["Settings:WorkflowServiceBaseURL"],
                    DataSourceManager.Instance.GetDataSourceLink(
                        DataSourceManager.Instance.GetDataSourceLocator())));


            services.AddSingleton<IMailServiceApi>(
                provider => new MailServiceClient(
                    configuration["Settings:MailServiceBaseURL"]));

            services.AddSingleton<ISearchServiceApi>(
                provider => new SearchServiceClient(
                    configuration["Settings:SearchServiceBaseURL"]));

            services.AddSingleton<IImportApi>(
                provider => new ImportServiceClient(
                    configuration["Settings:ImportServiceBaseURL"]));

            services.AddSingleton<IScheduleServiceWebApi>(
                provider => new ScheduleServiceWebClient(
                    configuration["Settings:ScheduleServiceBaseURL"]));

            return services;
        }
        
        public static IServiceCollection UseMsSql(this IServiceCollection services)
        {
            services.UseMsSqlDatabase();
            services.AddScoped<IDbConfiguration, MsSqlDbConfiguration>();
            return services;
        }
        
        public static IServiceCollection UsePgSql(this IServiceCollection services)
        {
            services.UsePgSqlDatabase();
            services.AddScoped<IDbConfiguration, PgSqlDbConfiguration>();
            return services;
        }

        public static IServiceCollection AddLookupQueries(this IServiceCollection services)
        {
            return services.AddMappingScoped(
                DAL.ServiceCollectionExtensions.DataAccessQueries
                    .AddBllLookupQueries());
        }

        public static IServiceCollection AddDemoWatcherService(this IServiceCollection services)
        {
            var standardKeyFilePath = Path.Combine("settings", "key.json");
            var keyFilePath = File.Exists(standardKeyFilePath)
                ? standardKeyFilePath
                : "key.json";
                
            if (!File.Exists(keyFilePath))
            {
                throw new DemoVersionException("Provide Key file");
            }
            
            var json = File.ReadAllText(keyFilePath);
            DemoLicenceObject licence;

            try
            {
                licence = JsonConvert.DeserializeObject<DemoLicenceObject>(json);
            }
            catch (Exception e)
            {
                throw new DemoVersionException("Invalid key format");
            }
            
            services.AddSingleton(licence);
         
            return services.AddHostedService<DemoWatcherService>();
        }
    }
}