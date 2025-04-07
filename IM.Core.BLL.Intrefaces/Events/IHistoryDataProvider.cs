using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Events
{
    /// <summary>
    /// Определяет интерфейс провайдера событий, связанных с объектами системы.
    /// </summary>
    public interface IHistoryDataProvider
    {
        /// <summary>
        /// Получить события, связанные с указанным объектом и удовлетворяющие фильтру.
        /// </summary>
        /// <param name="objectID">Уникальный идентификатор объекта.</param>
        /// <param name="classID">Сласс объекта.</param>
        /// <param name="filter">Фильтр событий.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Список событий, удовлетворяющих фильтру.</returns>
        Task<HistoryItemDetails[]> GetHistoryByObjectAsync(
            Guid objectID,
            ObjectClass? classID,
            HistoryFilter filter,
            CancellationToken cancellationToken = default);
    }
}
