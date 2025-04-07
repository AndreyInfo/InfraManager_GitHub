using IM.Core.ScheduleBLL.Interfaces;
using InfraManager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Accounts;
using InfraManager.ServiceBase.ScheduleService;
using Microsoft.Extensions.Logging;

namespace IM.Core.Schedule.BLL.Jobs.ImportJobs;

[JobSettings(true)]
public abstract class StartImportJobBase : IBaseJob
{
    private readonly IFinder<UserAccount> _accountFinder;
    private readonly ILogger<StartImportJobBase> _logger;
    
    public StartImportJobBase(IFinder<UserAccount> accountFinder,
        ILogger<StartImportJobBase> logger)
    {
        _accountFinder = accountFinder;
        _logger = logger;
    }

    public async Task ExecuteAsync(ScheduleTaskEntity task, CancellationToken cancellationToken = default)
    {
        var accountName = string.Empty;
        var password = string.Empty;
        if (task.UseAccount && task.CredentialID.HasValue)
        {
            var account = await _accountFinder.FindAsync(task.CredentialID, cancellationToken);
        
            accountName = account?.Login;
            password = account?.Password;
        }

        var importTaskRequest =
            new ImportTaskRequest(task.ID, task.TaskSettingID, task.UseAccount, accountName, password);
        
        ApiCall(importTaskRequest);
        
        _logger.LogInformation("Import task with with ID = {TaskID} was started", task.ID);
    }

    protected abstract Task ApiCall(ImportTaskRequest request);
}