using IM.Core.ScheduleBLL.Interfaces;
using InfraManager;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;

namespace IM.Core.Schedule.BLL.Jobs;

public class JobExecutor : IJobExecutor, ISelfRegisteredService<IJobExecutor>
{
    private readonly IServiceMapper<TaskTypeEnum, IBaseJob> _jobMapper;
    private readonly ILogger<JobExecutor> _logger;
    private readonly IAfterExecuteJobProcessor _afterExecuteJobProcessor;
    
    public JobExecutor(IServiceMapper<TaskTypeEnum, IBaseJob> jobMapper,
        ILogger<JobExecutor> logger,
        IAfterExecuteJobProcessor afterExecuteJobProcessor)
    {
        _jobMapper = jobMapper;
        _logger = logger;
        _afterExecuteJobProcessor = afterExecuteJobProcessor;
    }
    
    public async Task ExecuteAsync(ScheduleTaskEntity task, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogTrace("Job with ID = {TaskID} has been started", task.ID);

            if (task.CurrentSchedule == null)
            {
                _logger.LogInformation("Cant start Job with ID = {TaskID} because current schedule not set", task.ID);
                task.TaskState = TaskStateEnum.Finished;
                return;
            }
            
            task.CurrentSchedule.LastAt = task.LastStartAt;
            task.LastStartAt = DateTime.UtcNow;
            task.TaskState = TaskStateEnum.Running;

            var job = _jobMapper.Map(task.TaskType);
            await job.ExecuteAsync(task, cancellationToken);

            var jobSetting = job.GetSettings();
            
            if (!jobSetting.IsLongRunning)
            {
                _afterExecuteJobProcessor.ProcessJobAfterExecute(task);
                _logger.LogInformation("Job with ID = {TaskID} completed successfully", task.ID);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Job with name = {TaskName} ended with error", task.Name);
            task.TaskState = TaskStateEnum.StartWithError;
        }
    }
}