using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DemoChecker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InfraManager.UI.Web.Services;

public class DemoWatcherService : BackgroundService
{

    private readonly ILogger<DemoWatcherService> _logger;
    private readonly DemoLicenceObject _licenceObject;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public DemoWatcherService(ILogger<DemoWatcherService> logger,
        DemoLicenceObject licenceObject,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _licenceObject = licenceObject;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            ValidateLicence();
            
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                ValidateLicence();
            }
        }
        catch (Exception e)
        {
            _hostApplicationLifetime.StopApplication();
            _logger.LogError(e, "Error while check Demo licence");
        }
    }

    private void ValidateLicence()
    {
        var validationResult = SignatureValidator.VerifyData(_licenceObject);

        if (!DateTime.TryParseExact(_licenceObject.TimeTo, "dd.MM.yyyy HH:mm:ss", CultureInfo.CurrentCulture,
                DateTimeStyles.None, out var toDate))
        {
            _logger.LogInformation("Cant parse date from demo key");
            validationResult = false;
        }
        
        if (!validationResult || toDate < DateTime.UtcNow)
        {
            _logger.LogError("Invalid or overdue Demo licence");
            _hostApplicationLifetime.StopApplication();
        }
    }
}