using InfraManager.BLL.Import;
using InfraManager.DAL;
using InfraManager.DAL.Accounts;
using InfraManager.ServiceBase.ScheduleService;
using Microsoft.Extensions.Logging;

namespace IM.Core.Schedule.BLL.Jobs.ImportJobs;

public class ITAssetImport : StartImportJobBase
{
    private readonly IImportApi _api;
    
    public ITAssetImport(IFinder<UserAccount> accountFinder,
        ILogger<StartImportJobBase> logger,
        IImportApi api) : base(accountFinder, logger)
    {
        _api = api;
    }

    protected override Task ApiCall(ImportTaskRequest request)
    {
        return _api.StartImportITAssetAsync(request);
    }
}