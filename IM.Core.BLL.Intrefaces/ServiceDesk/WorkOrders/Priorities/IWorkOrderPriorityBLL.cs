using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Priorities;

/// <summary>
/// Бизнес логика для шаблонов заданий
/// </summary>
public interface IWorkOrderPriorityBLL
{
    /// <summary>
    /// Получение списка моделей, с возможностью поиска по имени
    /// </summary>
    /// <param name="filterBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WorkOrderPriorityDetails[]> GetDetailsArrayAsync(
          LookupListFilter filterBy,
          CancellationToken cancellationToken = default);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        
    Task<WorkOrderPriorityDetails> AddAsync(WorkOrderPriorityData data, CancellationToken cancellationToken = default);
        
    Task<WorkOrderPriorityDetails> UpdateAsync(Guid id, WorkOrderPriorityData data, CancellationToken cancellationToken = default);

    Task<WorkOrderPriorityDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
}
