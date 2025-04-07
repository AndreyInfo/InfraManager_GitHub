using InfraManager.BLL.Location;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.BLL.ProductCatalogue.Tree;
using InfraManager.DAL.AccessManagement;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.AccessManagement;

/// <summary>
/// Деревья доступа отвественности
/// </summary>
public interface IResponsibilityAccessTreeBLL
{
    /// <summary>
    /// Получение деревье доступа отвественности связанные с расположением
    /// </summary>
    /// <param name="filter">фильтр получения дерева</param>
    /// <param name="ownerID">идентификатор для кого получаем доступ отвественности</param>
    /// <param name="accessTypes">тип доступа</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы по parendID</returns>
    Task<LocationTreeNodeDetails[]> GetLocationTreeAsync(LocationTreeFilter filter
        , AccessFilter accessFilter
        , AccessTypes accessTypes
        , CancellationToken cancellationToken);

    /// <summary>
    /// Получение дерева доступа отвественности связанные с Оргструктурой
    /// </summary>
    /// <param name="filter">фильтр получения дерева</param>
    /// <param name="ownerID">идентификатор для кого получаем доступ отвественности</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы по parendID</returns>
    Task<OrganizationStructureNodeModelDetails[]> GetOrgstructureAsync(OrganizationStructureNodeRequestModelDetails filter
        , AccessFilter accessFilter
        , CancellationToken cancellationToken);

    /// <summary>
    /// Получение дерева доступа отвественности связанные с Портфелем сервисов
    /// Доступ к сервисам
    /// </summary>
    /// <param name="filter">фильтр получения дерева</param>
    /// <param name="ownerID">идентификатор для кого получаем доступ отвественности</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы по parendID</returns>
    Task<PortfolioServicesItem[]> GetTreePortfolioServiceAsync(PortfolioServiceFilter filter
        , AccessFilter accessFilter
        , CancellationToken cancellationToken);

    /// <summary>
    /// Получение дерева доступа отвественности связанные с Каталогом продуктов
    /// </summary>
    /// <param name="filter">фильтр получения дерева</param>
    /// <param name="ownerID">идентификатор для кого получаем доступ отвественности</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы по parendID</returns>
    Task<ProductCatalogNode[]> GetTreeProductCatalogAsync(ProductCatalogTreeFilter filter
        , AccessFilter accessFilter
        , CancellationToken cancellationToken);
}
