using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk;

/// <summary>
/// Предоставляет метод получения списка возможных исполнителей для назначения на заявку / задание / etc.
/// </summary>
/// <typeparam name="TItem">Тип объектов списка.</typeparam>
/// <typeparam name="TFilter">Тип фильтра.</typeparam>
public interface IFindExecutorBLL<TItem, in TFilter>
{
    /// <summary>
    /// Получить список исполнителей, которые удовлетворяют заданному фильтру.
    /// </summary>
    /// <param name="filter">Фильтр списка.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Массив объектов <see cref="TItem"/>.</returns>
    public Task<TItem[]> FindAsync(TFilter filter, CancellationToken cancellationToken = default);
}