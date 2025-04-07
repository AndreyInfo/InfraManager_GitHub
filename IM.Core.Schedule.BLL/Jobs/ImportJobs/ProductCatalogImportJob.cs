using InfraManager.BLL.Import;
using InfraManager.DAL;
using InfraManager.DAL.Accounts;
using InfraManager.ServiceBase.ScheduleService;
using Microsoft.Extensions.Logging;

namespace IM.Core.Schedule.BLL.Jobs.ImportJobs;

[JobSettings(true)]
public class ProductCatalogImportJob : StartImportJobBase
{
    private readonly IImportApi _api;

    public ProductCatalogImportJob(IImportApi api,
        IFinder<UserAccount> accountFinder,
        ILogger<ProductCatalogImportJob> logger) : base(accountFinder, logger)
    {
        _api = api;
    }

    protected override Task ApiCall(ImportTaskRequest request)
    {
#pragma warning disable CS4014
        return _api.StartImportServiceCatalogueAsync(request);
#pragma warning restore CS4014
    }
}