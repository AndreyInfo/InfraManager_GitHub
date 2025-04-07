using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceCatalog;

public interface IServiceDependencyQuery
{
    /// <summary>
    /// Получение зависимостей сервисов
    /// </summary>
    /// <param name="filter">фильтр для пагинации</param>
    /// <param name="sortProperty">параметр сортировки</param>
    /// <param name="parentID">id сервиса для которого получаем зависимости</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модели сервисов</returns>
    Task<ServiceModelItem[]> ExecuteQueryServiceDependencyAsync(PaggingFilter filter, Sort sortProperty, Guid? parentID, CancellationToken cancellationToken = default);
}
