using IM.Core.ScheduleBLL.Interfaces;
using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Schedule.BLL.Jobs;

public class JobManager : IJobManager, ISelfRegisteredService<IJobManager>
{        
    private readonly IRepository<ScheduleTaskEntity> _repository;
    private ScheduleTaskEntity[] _tasks;
    private readonly IJobExecutor _executor;
    private readonly IUnitOfWork _saveChanges;

    public JobManager(IRepository<ScheduleTaskEntity> repository,
        IJobExecutor executor, 
        IUnitOfWork saveChanges)
    {
        _repository = repository;
        _executor = executor;
        _saveChanges = saveChanges;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _tasks = await _repository.With(x => x.CurrentSchedule).WithMany(x => x.Schedules).ToArrayAsync(
            p => p.IsEnabled && p.TaskState == TaskStateEnum.Waiting && p.NextRunAt <= DateTime.UtcNow,
            cancellationToken);

        await ExecuteTasksAsync(cancellationToken);
    }

    private async Task ExecuteTasksAsync(CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();
        
        foreach (var el in _tasks)
        {
            tasks.Add(_executor.ExecuteAsync(el, cancellationToken));
        }

        await Task.WhenAll(tasks);
        
        if (_tasks.Length > 0)
        {
            await _saveChanges.SaveAsync(cancellationToken);
        }
    }
}