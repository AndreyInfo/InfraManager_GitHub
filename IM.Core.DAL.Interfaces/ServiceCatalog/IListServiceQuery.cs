using InfraManager.DAL.ServiceCatalog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceCatalogue;


public interface IListServiceQuery
{
    /// <summary>
    /// Получение списка сервисов по категории или просто подряд
    /// </summary>
    /// <param name="filter">фильтр для поиска и пагинации</param>
    /// <param name="sortProperty">параметр сортировки</param>
    /// <param name="categoryID">идентификатор сортировки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модели сервисов</returns>
    Task<ServiceModelItem[]> ExecuteAsync(PaggingFilter filter, Sort sortProperty, Guid? categoryID, CancellationToken cancellationToken = default);
}