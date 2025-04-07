using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Events
{
    /// <summary>
    /// Определяет интерфейс построителя запроса для получения событий по объекту.
    /// </summary>
    public interface IEventsForObjectQuery
    {
        /// <summary>
        /// Получить плоский список событий по условиям в заданном запросе.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Массив объектов <see cref="EventForObjectQueryResultItem"/>, удовлетворяющих запросу.</returns>
        Task<EventForObjectQueryResultItem[]> QueryAsync(EventsForObjectQueryRequest request, CancellationToken cancellationToken = default);
    }
}
