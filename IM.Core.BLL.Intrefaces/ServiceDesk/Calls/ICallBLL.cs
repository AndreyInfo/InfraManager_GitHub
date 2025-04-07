using Inframanager.BLL.ListView;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public interface ICallBLL
    {
        /// <summary>
        /// Получать данные заявки
        /// </summary>
        /// <param name="id">Идентификатор заявки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные заявки</returns>
        Task<CallDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Добавляет заявку
        /// </summary>
        /// <param name="data">Данные заявки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные новой заявки</returns>
        Task<CallDetails> AddAsync(CallData data, CancellationToken cancellationToken = default);
        /// <summary>
        /// Изменяет заявку
        /// </summary>
        /// <param name="id">Данные заявки</param>
        /// <param name="data">Данные, которые надо изменить</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные заявки после изменения</returns>
        Task<CallDetails> UpdateAsync(Guid id, CallData data, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет заявку
        /// </summary>
        /// <param name="id">Идентификатор заявки</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Список "Все заявки"
        /// </summary>
        /// <param name="filterBy">Фильтр данных списка</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные списка</returns>
        Task<CallListItem[]> AllCallsAsync(ListViewFilterData<ServiceDeskListFilter> filterBy, CancellationToken cancellationToken = default);
        /// <summary>
        /// Список "Мои заявки"
        /// </summary>
        /// <param name="filterBy">Фильтр данных списка</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные списка</returns>
        Task<CallFromMeListItem[]> CallsFromMeAsync(ListViewFilterData<CallFromMeListFilter> filterBy, CancellationToken cancellationToken = default);
        /// <summary>
        /// Список заявок, доступных для связывания с массовым инцидентом
        /// </summary>
        /// <param name="filterBy">Критерии выборки и сортировки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных списка</returns>
        Task<CallReferenceListItem[]> GetAvailableMassIncidentReferencesAsync(InframanagerObjectListViewFilter filterBy, CancellationToken cancellationToken = default);

        Task<CallDetails[]> GetDetailsArrayAsync(CallListFilter filter, CancellationToken cancellationToken = default);
    }
}
