using System.Threading;
using System;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
/// <summary>
/// Интерфейс выполнения команды жизненного цикла с алертом.
/// </summary>
public interface ILifeCycleCommandWithAlertExecutor
{
    /// <summary>
    /// Выполнение команды жизненного цикла с алертом.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="data">Данные команды жизненного цикла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат команды.</returns>
    public Task<LifeCycleCommandResultItem> ExecuteAsync(Guid id, LifeCycleCommandBaseData data, CancellationToken cancellationToken);
}
