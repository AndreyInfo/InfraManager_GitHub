using Inframanager.BLL.ListView;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public interface IWorkOrderBLL
    {
        Task<WorkOrderListItem[]> GetAllWorkOrdersAsync(ListViewFilterData<ServiceDeskListFilter> filterBy, CancellationToken cancellationToken = default);
        /// <summary>
        /// Возвращает данные списка "Инвентаризация"
        /// </summary>
        /// <param name="filterBy">Критерии выборки и сортировки данных</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<InventoryListItem[]> GetInventoryReportAsync (ListViewFilterData<ServiceDeskListFilter> filterBy, CancellationToken cancellationToken = default);
        Task<WorkOrderDetails[]> GetDetailsPageAsync(WorkOrderListFilter filter, CancellationToken cancellationToken = default);
        Task<WorkOrderDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<WorkOrderDetails> AddAsync(WorkOrderData data, CancellationToken cancellation = default);
        Task<WorkOrderDetails> UpdateAsync(Guid id, WorkOrderData data, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<WorkOrderDetails[]> GetDetailsArrayAsync(WorkOrderListFilter filterBy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить список связанных с Заданием объектов асинхронно.
        /// </summary>
        /// <param name="id">Уникальный идентификатор Задания.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Список связей Задания.</returns>
        Task<WorkOrderReference[]> GetReferencesAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Список заданий, доступных для связывания с массовым инцидентом
        /// </summary>
        /// <param name="filterBy">Критерии выборки и сортировки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных списка</returns>
        Task<WorkOrderReferenceListItem[]> GetAvailableMassIncidentReferencesAsync(
            InframanagerObjectListViewFilter filterBy,
            CancellationToken cancellationToken = default);

        Task<ReferencedWorkOrderListItem[]> Get(ListViewFilterData<WorkOrderListFilter> filter,
            CancellationToken cancellationToken = default);
    }
}
