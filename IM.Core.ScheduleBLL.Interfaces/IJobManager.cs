namespace IM.Core.ScheduleBLL.Interfaces;

/// <summary>
/// Интерфейс описывает работу менеджера сервиса планировщика
/// </summary>
public interface IJobManager
{
    /// <summary>
    /// Начинает выполнение задач, время которых подошло к выполнению
    /// </summary>
    /// <param name="cancellationToken">токен отмены</param>
    Task StartAsync(CancellationToken cancellationToken = default);
}