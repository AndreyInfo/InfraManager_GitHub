using System;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceDependencies;

public interface IServiceDependencyBLL
{
    /// <summary>
    /// Получение связанных сервисов с сервисом
    /// </summary>
    /// <param name="filter">фильтр для поиска и пагинации</param>
    /// <param name="parentId">идентификатор сервиса для которого получаем связанные сервисы</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модели сервисов</returns>
    public Task<InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services.ServiceDetails[]> GetTableAsync(BaseFilter filter, Guid? parentId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляем связи
    /// </summary>
    /// <param name="models">модели удаляемых связей</param>
    /// <param name="cancellationToken"></param>
    public Task<Guid[]> DeleteAsync(ServiceDependencyModel[] models, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление связи
    /// </summary>
    /// <param name="parentId">для кого добавляем связь</param>
    /// <param name="childId">с кем связываем</param>
    /// <param name="cancellationToken"></param>
    public Task<bool> AddAsync(Guid parentId, Guid childId, CancellationToken cancellationToken);
}
