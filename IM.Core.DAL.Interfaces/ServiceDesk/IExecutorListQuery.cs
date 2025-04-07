using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Предоставляет метод для получения списка исполниетелей заданного типа <typeparamref name="TExecutor"/>.
/// </summary>
/// <typeparam name="TQueryResultItem">Тип результата запроса.</typeparam>
/// <typeparam name="TExecutor">Тип исполнителя.</typeparam>
public interface IExecutorListQuery<TQueryResultItem, TExecutor>
{
    /// <summary>
    /// Получить список исполнителей удовлетворяющих фильтру.
    /// </summary>
    /// <param name="filter">Фильтр.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Массив объектов <see cref="TQueryResultItem"/>.</returns>
    Task<TQueryResultItem[]> ExecuteAsync(ExecutorListQueryFilter filter, CancellationToken cancellationToken = default);
}