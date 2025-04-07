using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Sessions;
using InfraManager.BLL.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InfraManager.UI.Web.Services;

public class SessionWatcherService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<SessionWatcherService> _logger;

    public SessionWatcherService(IServiceProvider services,
        ILogger<SessionWatcherService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var timerPeriodic = await GetPeriodicTimeAsync(stoppingToken);

            var timer = new PeriodicTimer(TimeSpan.FromSeconds(timerPeriodic));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var systemSessionBLL =
                            scope.ServiceProvider
                                .GetRequiredService<ISystemSessionBLL>();

                        await systemSessionBLL.DeactivateInactiveSessionsAsync(stoppingToken);

                        var currentPeriodic = await GetPeriodicTimeAsync(stoppingToken);

                        if (timerPeriodic != currentPeriodic)
                        {
                            timer = new PeriodicTimer(TimeSpan.FromSeconds(currentPeriodic));
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while executing session watcher");
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deactivate inactive sessions");
        }
    }

    private async Task<int> GetPeriodicTimeAsync(CancellationToken cancellationToken = default)
    {
        var defaultPeriodicTime = 30;
        using (var scope = _services.CreateScope())
        {
            var settingBLL = scope.ServiceProvider.GetRequiredService<ISettingsBLL>();
            var converter = scope.ServiceProvider.GetRequiredService<IConvertSettingValue<int>>();

            var periodicTimeFromDB =
                converter.Convert(await settingBLL.GetValueAsync(SystemSettings.PeriodicTimeCheck, cancellationToken));

            if (periodicTimeFromDB > 0)
            {
                defaultPeriodicTime = periodicTimeFromDB;
            }
        }

        return defaultPeriodicTime;
    }
}