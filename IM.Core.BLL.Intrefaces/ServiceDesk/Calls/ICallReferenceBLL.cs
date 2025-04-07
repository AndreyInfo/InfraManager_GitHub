using InfraManager.BLL.ServiceDesk.MassIncidents;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    /// <summary>
    /// Этот интерфейс описывает сервис BLL сущности "Связь с заявкой"
    /// </summary>
    public interface ICallReferenceBLL
    {
        /// <summary>
        /// Возвращает список связей заявки с объектами TReference, удовлетворяющих критериям выборки
        /// </summary>
        /// <param name="filterBy">Ссылка на критерии выборки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив связей заявок и объектов TReference</returns>
        Task<CallReferenceData[]> GetAsync(CallReferenceListFilter filterBy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавляет новую связь заявки с объектом TReference
        /// </summary>
        /// <param name="reference">Данные новой связи с заявкой</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные новой связт с заявкой</returns>
        Task<CallReferenceData> AddAsync(CallReferenceData reference, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет связь заявки с объектом TReference
        /// </summary>
        /// <param name="reference">Ключ пары заявка - связанный объект</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(CallReferenceData reference, CancellationToken cancellationToken = default);

        /// <summary>
        /// Работает так же как и GetAsync, но поддерживает таблицу, а не обычный список
        /// </summary>
        /// <param name="filterBy">Фильтр для пагинации и таблицы</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<CallListItem[]> GetReferencesAsync(CallReferenceListFilter filterBy,
            CancellationToken cancellationToken = default);
    }
}
