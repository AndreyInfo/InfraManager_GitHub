using IM.Core.ScheduleBLL.Interfaces;
using InfraManager.DAL;

namespace IM.Core.Schedule.BLL.Jobs.ScheduleCalculators;

public abstract class BaseScheduleCalculator : IBaseScheduleCalculator
{
    public abstract void Calculate(ScheduleEntity schedule);

    public bool IsSatisfied(ScheduleEntity schedule)
    {
        return !(schedule.FinishAt.HasValue && schedule.FinishAt < DateTime.UtcNow) && AdditionalSatisfaction(schedule);
    }

    protected virtual bool AdditionalSatisfaction(ScheduleEntity schedule)
    {
        return true;
    }
}