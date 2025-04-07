using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    /// <summary>
    /// Этот интерфейс описывает сервис слоя бизнес логики для типов запросов на изменения
    /// </summary>
    public interface IRfcTypeBLL
    {
        /// <summary>
        /// Получает набор данных типов запросов на изменения, удовлетворяющих условию выборки
        /// </summary>
        /// <param name="filter">фильтр выборки</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Массив объектов-моделей</returns>
        Task<RfcTypeDetailsModel[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken = default);
        
        Task<RfcTypeDetailsModel> AddAsync(RfcTypeModel newRfcType, CancellationToken cancellationToken = default);
        
        Task<RfcTypeDetailsModel> UpdateAsync(Guid id, RfcTypeModel newRfcTypeState, CancellationToken cancellationToken = default);
        
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<RfcTypeDetailsModel> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<RfcTypeDetailsModel[]> GetDetailsArrayAsync(LookupListFilter filterBy,
            CancellationToken cancellationToken = default);

    }
}
