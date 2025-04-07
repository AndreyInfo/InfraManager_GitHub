using InfraManager.DAL;

namespace IM.Core.ScheduleBLL.Interfaces;

/// <summary>
/// Интерфейс описывает как будет обработана задача после выполнения
/// </summary>
public interface IAfterExecuteJobProcessor
{
    /// <summary>
    /// Обрабатывает задачу после выполнения
    /// </summary>
    /// <param name="task">Задача, которая выполнилась</param>
    void ProcessJobAfterExecute(ScheduleTaskEntity task);
}