using Inframanager.BLL.ListView;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    public interface IChangeRequestBLL
    {
        Task<ChangeRequestListItem[]> GetChangeRequestsAsync(ListViewFilterData<ServiceDeskListFilter> filterBy, CancellationToken cancellationToken = default);

        Task<ChangeRequestDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<ChangeRequestDetails> AddAsync(ChangeRequestData problem, CancellationToken cancellationToken = default);

        Task<ChangeRequestDetails> UpdateAsync(Guid id, ChangeRequestData problem, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить выборку запросов на изменения по фильтру
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns></returns>
        Task<ChangeRequestDetails[]> GetDetailsArrayAsync(ChangeRequestListFilter filter, CancellationToken cancellationToken = default);
        /// <summary>
        /// Список запросов на изменения, доступных для связывания с массовым инцидентом
        /// </summary>
        /// <param name="filterBy">Критерии выборки и сортировки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных списка</returns>
        Task<ChangeRequestReferenceListItem[]> GetAvailableMassIncidentReferencesAsync(
            InframanagerObjectListViewFilter filterBy, 
            CancellationToken cancellationToken = default);
    }
}
