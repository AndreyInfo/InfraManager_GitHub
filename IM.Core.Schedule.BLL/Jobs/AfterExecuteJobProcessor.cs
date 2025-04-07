using IM.Core.ScheduleBLL.Interfaces;
using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Schedule.BLL.Jobs;

public class AfterExecuteJobProcessor : IAfterExecuteJobProcessor, ISelfRegisteredService<IAfterExecuteJobProcessor>
{
    private readonly IScheduleCalculator _scheduleCalculator;

    public AfterExecuteJobProcessor(IScheduleCalculator scheduleCalculator)
    {
        _scheduleCalculator = scheduleCalculator;
    }
    
    public void ProcessJobAfterExecute(ScheduleTaskEntity task)
    {
        task.TaskState = TaskStateEnum.Waiting;
        task.FinishRunAt = DateTime.UtcNow;
        task.CurrentSchedule.LastAt = task.FinishRunAt;

        var currentSchedule = _scheduleCalculator.CalculateNextSchedule(task);
        
        if (currentSchedule != null)
        {
            task.CurrentExecutingScheduleID = currentSchedule.ID;
            task.NextRunAt = task.CurrentSchedule!.NextAt;
        }
        else
        {
            task.CurrentExecutingScheduleID = null;
        }
    }
}