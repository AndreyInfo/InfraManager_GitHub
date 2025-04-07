using System;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

public interface IPortfolioServiceBLL
{
    /// <summary>
    /// Получение дерева портфеля сервисов
    /// </summary>
    /// <param name="filter">фильтр дерева</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы дерева</returns>
    Task<PortfolioServicesItem[]> GetTreeAsync(PortfolioServiceFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение заказчика
    /// </summary>
    /// <param name="classID">тип заказкчика</param>
    /// <param name="id">идентификатор заказчика</param>
    /// <returns>модель заказчика</returns>
    ServiceCustomerDetails GetCustomer(ObjectClass classID, Guid id);

    /// <summary>
    /// Получение не заказчиков
    /// </summary>
    /// <param name="classIDs"></param>
    /// <param name="ids"></param>
    /// <param name="search">строка поиска</param>
    /// <returns>модели не заказчиков</returns>
    ServiceCustomerDetails[] GetNotCustomer(ObjectClass[] classIDs, Guid[] ids, string search);

    /// <summary>
    /// Получение пути узла в дереве
    /// </summary>
    /// <param name="classID">тип узла</param>
    /// <param name="id">идентификатор узла</param>
    /// <returns>массив узлов</returns>
    PortfolioServicesItem[] GetPath(ObjectClass classID, Guid id);

    /// <summary>
    /// Добавление инфраструктуры
    /// </summary>
    /// <param name="model">модель связи</param>
    /// <param name="cancellationToken"></param>
    /// <returns>идентификатор инфраструктуры</returns>
    Task<Guid> AddInfrastructureAsync(ServiceReferenceModel model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение инфраструктуры для сервиса
    /// </summary>
    /// <param name="serviceID">идентификатор сервиса</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модели инфраструктуры</returns>
    Task<PortfolioServiceInfrastructureItem[]> GetInfrastructureAsync(Guid serviceID, CancellationToken cancellationToken = default);
}
