using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Import;
using InfraManager.DAL.Microsoft.SqlServer.ModelBuilders;
using InfraManager.DAL.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.DAL.Microsoft.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseMsSqlDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionStringProvider, MsSqlConnectionStringProvider>();

            services.AddScoped<IConfigureDbContext>(
                provider =>
                {
                    var configuration = provider.GetService<IConfiguration>();
                    var connectionStringProvider = provider.GetService<IConnectionStringProvider>();
                    return new DbContextConfigurer(
                        connectionStringProvider.GetConnectionString(),
                        configuration["ms-schema"]);
                });
            services.AddScoped<IBuildDbContextModel, DbContextModelBuilder>();


            return services;
        }

    }
}
