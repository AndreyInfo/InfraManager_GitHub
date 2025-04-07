using InfraManager.DAL;

namespace IM.Core.Schedule.BLL.Jobs.ScheduleCalculators;

public class ImmediatelyScheduleCalculator : BaseScheduleCalculator
{
    public override void Calculate(ScheduleEntity schedule)
    {
        schedule.NextAt = schedule.StartAt;
    }

    protected override bool AdditionalSatisfaction(ScheduleEntity schedule)
    {
        return schedule.LastAt == null;
    }
}