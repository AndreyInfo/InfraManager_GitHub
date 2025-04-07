using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
/// <summary>
/// Интерфейс выполнения команд жизненного цикла в зависимости от операции.
/// </summary>
public interface ILifeCycleCommandFacade
{
    /// <summary>
    /// Выполнение команды в зависимости от операции.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="operationID">Идентификатор операции.</param>
    /// <param name="data">Данные команды жизненного цикла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task ExecuteAsync(Guid id, Guid operationID, LifeCycleCommandBaseData data, CancellationToken cancellationToken);

    public Task<LifeCycleCommandAlertDetails> ExecuteWithAlertAsync(Guid id
        , Guid operationID
        , LifeCycleCommandBaseData data
        , CancellationToken cancellationToken);
}
