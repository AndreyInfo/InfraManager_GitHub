using InfraManager.DAL;

namespace IM.Core.ScheduleBLL.Interfaces;

/// <summary>
/// Интерфейс описывает работу рассчитывания времени для разных типов расписания
/// </summary>
public interface IScheduleCalculator
{
    /// <summary>
    /// Возвращает самое близкое расписание для выполнения
    /// </summary>
    ScheduleEntity CalculateNextSchedule(ScheduleTaskEntity task);
}