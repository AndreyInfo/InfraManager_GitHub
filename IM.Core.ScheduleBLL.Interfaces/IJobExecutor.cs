using InfraManager.DAL;

namespace IM.Core.ScheduleBLL.Interfaces;

/// <summary>
/// Интерфейс описывает функционал выполнителя заданий
/// </summary>
public interface IJobExecutor
{
    /// <summary>
    /// В зависимости от типа задания использует нужную <see cref="IBaseJob"/> для выполнения задания
    /// </summary>
    /// <param name="task">Задача, которую нужно выполнить</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task ExecuteAsync(ScheduleTaskEntity task, CancellationToken cancellationToken = default);
}