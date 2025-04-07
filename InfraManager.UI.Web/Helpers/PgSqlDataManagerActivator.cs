using InfraManager.DAL.Data;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Helpers
{
    public class PgSqlDataManagerActivator : IHostedService
    {
        public PgSqlDataManagerActivator(IServiceProvider serviceProvider)
        {
            PostgreSqlDataSource.ServiceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
