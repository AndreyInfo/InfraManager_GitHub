using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
/// <summary>
/// Интерфейс выполнения команды жизненного цикла.
/// </summary>
public interface ILifeCycleCommandExecutor
{
    /// <summary>
    /// Выполнение команды жизненного цикла.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="data">Данные команды жизненного цикла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения команды.</returns>
    public Task<LifeCycleCommandResultItem> ExecuteAsync(Guid id, LifeCycleCommandBaseData data, CancellationToken cancellationToken);
}
