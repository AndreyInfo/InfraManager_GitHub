using InfraManager.DAL;

namespace IM.Core.Schedule.BLL.Jobs.ScheduleCalculators;

public class MonthlyScheduleCalculator : BaseScheduleCalculator
{
    private bool[] ConvertToBool(string months)
    {
        var resultMonths = new bool[12];

        var convertedToIntWeeks = months.Split(',').Select(int.Parse).ToArray();
        foreach (var el in convertedToIntWeeks)
        {
            resultMonths[el - 1] = true;
        }

        return resultMonths;
    }
    
    public override void Calculate(ScheduleEntity schedule)
    {
        var utcNow = DateTime.UtcNow;
        var utcStartAt = schedule.StartAt!.Value;
        var monthsToRun = ConvertToBool(schedule.Months);
        var monthDay = schedule.Interval;
        DateTime utcNextRunAt;

        if (utcStartAt <= utcNow)
            utcNextRunAt = new DateTime(utcNow.Year, utcNow.Month, 1, utcStartAt.Hour, utcStartAt.Minute,
                utcStartAt.Second, DateTimeKind.Utc);
        else
            utcNextRunAt = new DateTime(utcStartAt.Year, utcStartAt.Month, 1, utcStartAt.Hour, utcStartAt.Minute,
                utcStartAt.Second, DateTimeKind.Utc);
        
        while (!monthsToRun[utcNextRunAt.Month - 1] ||
               DateTime.DaysInMonth(utcNextRunAt.Year, utcNextRunAt.Month) < monthDay ||
               utcNextRunAt.AddDays(monthDay - 1) < utcNow)
        {
            utcNextRunAt = utcNextRunAt.AddMonths(1);
            utcNextRunAt = utcNextRunAt.AddDays(monthDay - 1).ToUniversalTime();
        }
        
        schedule.NextAt = utcNextRunAt;
    }
}