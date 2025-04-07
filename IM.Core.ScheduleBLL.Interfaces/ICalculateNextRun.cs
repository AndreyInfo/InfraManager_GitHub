using InfraManager.DAL;

namespace IM.Core.ScheduleBLL.Interfaces;

/// <summary>
/// Интерфейс описывает функционал расчета следующей даты выполнения задания
/// </summary>
public interface ICalculateNextRun
{
    /// <summary>
    /// Рассчитывает следующую дату выполнениея задания
    /// </summary>
    /// <param name="task">Задание для расчета</param>
    /// <returns>Дата следующего выполнения</returns>
    DateTime Calculate(ScheduleTaskEntity task);
}