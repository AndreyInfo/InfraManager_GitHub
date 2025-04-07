using IM.Core.DAL.Postgres;
using InfraManager.DAL;
using InfraManager.DAL.Import;
using InfraManager.DAL.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IM.Core.DAL.PGSQL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UsePgSqlDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionStringProvider, PgSqlConnectionStringProvider>();
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
