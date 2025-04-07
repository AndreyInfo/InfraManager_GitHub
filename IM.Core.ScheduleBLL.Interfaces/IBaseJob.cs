using InfraManager.DAL;

namespace IM.Core.ScheduleBLL.Interfaces;

/// <summary>
/// Интерфейс описывает базовую реализацию задания
/// </summary>
public interface IBaseJob
{
    /// <summary>
    /// Начинает выполнение команды
    /// </summary>
    /// <param name="task">Задание, которое требуется выполнить</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task ExecuteAsync(ScheduleTaskEntity task, CancellationToken cancellationToken = default);
}