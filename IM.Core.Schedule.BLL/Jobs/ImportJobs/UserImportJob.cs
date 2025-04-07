using InfraManager.BLL.Import;
using InfraManager.DAL;
using InfraManager.DAL.Accounts;
using InfraManager.ServiceBase.ScheduleService;
using Microsoft.Extensions.Logging;

namespace IM.Core.Schedule.BLL.Jobs.ImportJobs;

[JobSettings(true)]
public class UserImportJob : StartImportJobBase
{
    private readonly IImportApi _api;

    public UserImportJob(IImportApi api,
        IFinder<UserAccount> accountFinder,
        ILogger<UserImportJob> logger) : base(accountFinder, logger)
    {
        _api = api;
    }

    protected override Task ApiCall(ImportTaskRequest request)
    {
#pragma warning disable CS4014
        return _api.ImportAsync(request); //no need to await task here
#pragma warning restore CS4014
    }
}