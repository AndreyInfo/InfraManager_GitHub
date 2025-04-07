using IM.Core.Schedule.BLL.Jobs.ScheduleCalculators;
using IM.Core.ScheduleBLL.Interfaces;
using InfraManager;
using InfraManager.DAL;
using InfraManager.Services.ScheduleService;

namespace IM.Core.Schedule.BLL.Jobs;

public class ScheduleCalculator : IScheduleCalculator, ISelfRegisteredService<IScheduleCalculator>
{
    private readonly IServiceMapper<ScheduleTypeEnum, BaseScheduleCalculator> _scheduleCalculators;
        
    public ScheduleCalculator(IServiceMapper<ScheduleTypeEnum, BaseScheduleCalculator> scheduleCalculators)
    {
        _scheduleCalculators = scheduleCalculators;
    }
    
    public ScheduleEntity CalculateNextSchedule(ScheduleTaskEntity task)
    {
        var schedules = new List<ScheduleEntity>();

        foreach (var el in task.Schedules)
        {
            var calculator = _scheduleCalculators.Map(el.ScheduleType);

            if (calculator.IsSatisfied(el))
            {
                schedules.Add(el);
                calculator.Calculate(el);
            }
        }
        
        if (schedules.Count == 0)
        {
            task.TaskState = TaskStateEnum.Finished;
            return null;
        }

        return schedules.FirstOrDefault(x =>
            x.NextAt == schedules.Where(x => x.NextAt < (x.FinishAt ?? DateTime.MaxValue)).Min(x => x.NextAt));
    }
}