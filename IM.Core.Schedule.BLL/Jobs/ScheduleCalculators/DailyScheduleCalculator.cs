using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Schedule.BLL.Jobs.ScheduleCalculators;

public class DailyScheduleCalculator : BaseScheduleCalculator
{
    public override void Calculate(ScheduleEntity schedule)
    {
        if (schedule.LastAt == null)
        {
            schedule.NextAt = schedule.StartAt;
            return;
        }
        
        if (schedule.NextAt < DateTime.UtcNow)
        {
            var nextDate = schedule.LastAt!.Value.AddDays(schedule.Interval)
                .ChangeTime(schedule.StartAt!.Value.TimeOfDay);

            schedule.NextAt = nextDate;
        }
    }
}