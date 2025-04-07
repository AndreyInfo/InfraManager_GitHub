using InfraManager.DAL;

namespace IM.Core.ScheduleBLL.Interfaces;

/// <summary>
/// Интерфейс описывает работу вычисления следующего времени выполнения задачи для определенного типа
/// </summary>
public interface IBaseScheduleCalculator
{
    /// <summary>
    /// Рассчитывает следующее время выполнение у расписание
    /// </summary>
    void Calculate(ScheduleEntity schedule);

    /// <summary>
    /// Проверяет, выполняются ли все требования для рассчета для этого расписания
    /// </summary>
    bool IsSatisfied(ScheduleEntity schedule);
}