using InfraManager.DAL;

namespace IM.Core.Schedule.BLL.Jobs.ScheduleCalculators;

public class WeeklyScheduleCalculator : BaseScheduleCalculator
{
    private bool[] ConvertToBool(string weeks)
    {
        var resultWeeks = new bool[7];

        var convertedToIntWeeks = weeks.Split(',').Select(int.Parse).ToArray();
        foreach (var el in convertedToIntWeeks)
        {
            resultWeeks[el] = true;
        }

        return resultWeeks;
    }
    
    public override void Calculate(ScheduleEntity schedule)
    {
        var utcStartAt = schedule.StartAt;
        var daysOfWeekToRun = ConvertToBool(schedule.DaysOfWeek);
        var interval = schedule.Interval;
        DateTime utcNextRunAt;

        var utcNow = DateTime.UtcNow;
        if (utcNow <= utcStartAt)
        {
            utcNextRunAt = utcStartAt.Value;
        }
        else
        {
            var days = utcNow.Subtract(utcStartAt!.Value).Days;
            var daysToAdd = (interval * 7) * (days / (interval * 7));
            utcNextRunAt = utcStartAt!.Value.AddDays(daysToAdd);
            if (utcNow.Subtract(utcNextRunAt).Days > 7)
                utcNextRunAt = utcNextRunAt.AddDays(interval * 7);
        }

        var dayOfWeek = (int)utcNextRunAt.DayOfWeek;
        var isInWeekFound = false;
        for (var dayIndex = 0; dayIndex < 7; dayIndex++)
            if (daysOfWeekToRun[(dayIndex + dayOfWeek) % 7])
                if (utcNow <= utcNextRunAt.AddDays(dayIndex))
                {
                    utcNextRunAt = utcNextRunAt.AddDays(dayIndex);
                    isInWeekFound = true;
                    break;
                }

        //Если на текущей неделе нет подходящих дней, то смотрим следующую настроенную неделю - там точно должно быть
        if (!isInWeekFound)
        {
            utcNextRunAt = utcNextRunAt.AddDays(interval * 7);
            for (var dayIndex = 0; dayIndex < 7; dayIndex++)
                if (daysOfWeekToRun[(dayIndex + dayOfWeek) % 7])
                    if (utcNow <= utcNextRunAt.AddDays(dayIndex))
                    {
                        utcNextRunAt = utcNextRunAt.AddDays(dayIndex);
                        isInWeekFound = true;
                        break;
                    }
        }
        
        schedule.NextAt = utcNextRunAt;
    }
}