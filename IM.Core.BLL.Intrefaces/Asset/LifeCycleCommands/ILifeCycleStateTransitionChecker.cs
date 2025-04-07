using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
/// <summary>
/// Интерфейс проверки условий перехода между состояниями жизненого цикла.
/// </summary>
public interface ILifeCycleStateTransitionChecker
{
    /// <summary>
    /// Получение состояния из условия перехода.
    /// </summary>
    /// <param name="operationID">Идентификатор операции.</param>
    /// <param name="item">Результат команды для сравнения с условиями.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор состояния или null в случае, если условия не выполнены/не заданы.</returns>
    public Task<Guid?> GetStateAsync(Guid operationID
        , LifeCycleCommandResultItem? item
        , CancellationToken cancellationToken);
}
