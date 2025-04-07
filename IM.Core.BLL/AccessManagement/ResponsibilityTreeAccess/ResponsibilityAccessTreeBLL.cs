using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfraManager.BLL.Location;
using InfraManager.DAL.AccessManagement;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.BLL.ProductCatalogue.Tree;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess;

internal class ResponsibilityAccessTreeBLL : IResponsibilityAccessTreeBLL
    , ISelfRegisteredService<IResponsibilityAccessTreeBLL>
{
    // TODO будут корректировки с фронта по бизнес логику, поменять названия методов
    // TODO обобщить методы, нужно сделать одинаковые поля ID, ClassID, IsSelectFull, IsSelectPart
    private readonly ILocationBLL _locationBll;
    private readonly IOrganizationStructureBLL _organizationStructureBLL;
    private readonly IPortfolioServiceBLL _portfolioServiceBLL;
    private readonly IObjectAccessBLL _objectAccessBLL;
    private readonly IProductCatalogTree _productCatalogTree;
    public ResponsibilityAccessTreeBLL(ILocationBLL locationBll
                                       , IOrganizationStructureBLL organizationStructureBLL
                                       , IPortfolioServiceBLL portfolioServiceBLL
                                       , IObjectAccessBLL objectAccessBLL
                                       , IProductCatalogTree productCatalogTree)
    {
        _locationBll = locationBll;
        _organizationStructureBLL = organizationStructureBLL;
        _portfolioServiceBLL = portfolioServiceBLL;
        _objectAccessBLL = objectAccessBLL;
        _productCatalogTree = productCatalogTree;
    }

    #region Дерево местоположений
    public async Task<LocationTreeNodeDetails[]> GetLocationTreeAsync(LocationTreeFilter filter
        , AccessFilter accessFilter
        , AccessTypes accessTypes
        , CancellationToken cancellationToken)
    {
        var nodes = await ExecuteGettingNodes(filter, cancellationToken);
        await InitializateSelectByResponsobilityAccessAsync(nodes, accessFilter, accessTypes, cancellationToken);
        return nodes;
    }

    private async Task<LocationTreeNodeDetails[]> ExecuteGettingNodes(LocationTreeFilter filter, CancellationToken cancellationToken) =>
        filter.ClassID switch
        {
            ObjectClass.Owner => await _locationBll.GetOwnerNodesAsync(cancellationToken),
            ObjectClass.Organizaton => await _locationBll.GetOrganizationNodesAsync(cancellationToken),
            ObjectClass.Building => await _locationBll.GetBuildingNodesAsync(filter.OrganizationID.Value, cancellationToken),
            ObjectClass.Floor => await _locationBll.GetFloorNodesAsync(filter.ParentID, cancellationToken),
            ObjectClass.Room => await _locationBll.GetRoomNodesAsync(filter.ParentID, filter.ChildClassID, cancellationToken),
            ObjectClass.Workplace => await _locationBll.GetWorkplaceNodesAsync(filter.ParentID, cancellationToken),
            ObjectClass.Rack => await _locationBll.GetRackNodesAsync(filter.ParentID, cancellationToken),
            _ => throw new Exception($"Not corrcet classID: {filter.ClassID}")
        };

    /// <summary>
    /// Инициализация полей IsSelectFull и IsSelectPart по доступу отвественности
    /// для деревьев в Доступ/Отвственности
    /// </summary>
    /// <param name="array">массив моделей к которым определяем доступ</param>
    /// <param name="accessFilter">фильтр доступа</param>
    /// <param name="accessTypes">тип доступа</param>
    /// <param name="cancellationToken"></param>
    private async Task InitializateSelectByResponsobilityAccessAsync(IEnumerable<LocationTreeNodeDetails> array
        , AccessFilter accessFilter
        , AccessTypes accessTypes
        , CancellationToken cancellationToken)
    {
        // Проверка значение AccessType для доступа/отвественности
        // Дерево местоположений переиспользуется в деревьях доступа/ответственности(дерево Объекты и дерево Местоположений),
        // поэтому проверяется используется оно там где нужно или фронт прокидывает не нужные данные
        if (accessTypes != AccessTypes.TOZ_sks && accessTypes != AccessTypes.TTZ_sks)
            throw new Exception($"Not correct AccessType to Tree location {accessTypes}");

        foreach (var item in array)
        {
            item.IsSelectFull = await _objectAccessBLL.AccessIsGrantedAsync(accessFilter.OwnerID
                                                                , accessFilter.OwnerClassID
                                                                , item.UID
                                                                , item.ClassID
                                                                , accessTypes
                                                                , true
                                                                , cancellationToken);
            item.IsSelectPart = !item.IsSelectFull
                && await _objectAccessBLL.AccessIsGrantedAsync(accessFilter.OwnerID
                                                               , accessFilter.OwnerClassID
                                                               , item.UID
                                                               , item.ClassID
                                                               , accessTypes
                                                               , cancellationToken: cancellationToken);
        }
    }
    #endregion

    #region Дерево оргструктуры
    public async Task<OrganizationStructureNodeModelDetails[]> GetOrgstructureAsync(
        OrganizationStructureNodeRequestModelDetails nodeRequest
        , AccessFilter accessFilter
        , CancellationToken cancellationToken)
    {
        var nodes = await _organizationStructureBLL.GetNodesAsync(nodeRequest, cancellationToken);
        await InitializateResponsobilityAccessAsync(nodes, accessFilter, cancellationToken);
        return nodes;
    }


    /// <summary>
    /// Инициализация полей IsSelectFull и IsSelectPart по доступу отвественности
    /// для деревв портфеля сервисов в Доступ/Отвственности
    /// </summary>
    /// <param name="array">массив обрабатываемых моделей, к которым определяется доступ</param>
    /// <param name="accessFilter">фильтр доступа</param>
    /// <param name="cancellationToken"></param>
    private async Task InitializateResponsobilityAccessAsync(IEnumerable<OrganizationStructureNodeModelDetails> array
        , AccessFilter accessFilter
        , CancellationToken cancellationToken)
    {
        foreach (var item in array)
        {
            item.IsSelectFull = await _objectAccessBLL.AccessIsGrantedAsync(accessFilter.OwnerID
                                                                , accessFilter.OwnerClassID
                                                                , item.ObjectID
                                                                , item.ClassID
                                                                , AccessTypes.TOZ_org
                                                                , true
                                                                , cancellationToken);
            item.IsSelectPart = !item.IsSelectFull
                && await _objectAccessBLL.AccessIsGrantedAsync(accessFilter.OwnerID
                                                               , accessFilter.OwnerClassID
                                                               , item.ObjectID
                                                               , item.ClassID
                                                               , AccessTypes.TOZ_org
                                                               , cancellationToken: cancellationToken);
        }
    }
    #endregion

    #region Портфель сервисов
    public async Task<PortfolioServicesItem[]> GetTreePortfolioServiceAsync(PortfolioServiceFilter filter
        , AccessFilter accessFilter
        , CancellationToken cancellationToken)
    {
        var nodes = await _portfolioServiceBLL.GetTreeAsync(filter, cancellationToken);
        await InitializateResponsobilityAccessAsync(nodes, accessFilter, cancellationToken);
        return nodes;
    }

    /// <summary>
    /// Инициализация полей IsSelectFull и IsSelectPart по доступу ответственности
    /// для дерева портфеля сервисов в Доступ/Ответственности
    /// </summary>
    /// <param name="array">массив обрабатываемых моделей, к которым определяется доступ</param>
    /// <param name="accessFilter">фильтр доступа</param>
    /// <param name="cancellationToken"></param>
    private async Task InitializateResponsobilityAccessAsync(PortfolioServicesItem[] array
        , AccessFilter accessFilter
        , CancellationToken cancellationToken)
    {
        foreach (var item in array)
        {
            item.IsSelectFull = await _objectAccessBLL.AccessIsGrantedAsync(accessFilter.OwnerID
                                                               , accessFilter.OwnerClassID
                                                               , item.ID.GetValueOrDefault()
                                                               , item.ClassId
                                                               , AccessTypes.ServiceCatalogue
                                                               , true
                                                               , cancellationToken);
            item.IsSelectPart = !item.IsSelectFull
                && await _objectAccessBLL.AccessIsGrantedAsync(accessFilter.OwnerID
                                                               , accessFilter.OwnerClassID
                                                               , item.ID.GetValueOrDefault()
                                                               , item.ClassId
                                                               , AccessTypes.ServiceCatalogue
                                                               , cancellationToken: cancellationToken);
        }
    }
    #endregion

    #region Дерево каталога продуктов
    public async Task<ProductCatalogNode[]> GetTreeProductCatalogAsync(ProductCatalogTreeFilter filter
        , AccessFilter accessFilter
        , CancellationToken cancellationToken)
    {
        var nodes = await _productCatalogTree.ExecuteAsync(filter, cancellationToken);
        await InitializateSelectDeviceAsync(nodes, accessFilter, cancellationToken);
        return nodes;
    }

    /// <summary>
    /// Определение, доступа сущности к девайсам в дереве каталога продуктов
    /// </summary>
    /// <param name="array">массив обрабатываемых моделей, к которым определяется доступ</param>
    /// <param name="accessFilter">фильтр доступа</param>
    /// <param name="cancellationToken"></param>
    private async Task InitializateSelectDeviceAsync(IEnumerable<ProductCatalogNode> array
        , AccessFilter accessFilter
        , CancellationToken cancellationToken)
    {
        foreach (var item in array)
        {
            item.IsSelectFull = await _objectAccessBLL.AccessIsGrantedAsync(accessFilter.OwnerID
                                                                 , accessFilter.OwnerClassID
                                                                 , item.ID
                                                                 , item.ClassID
                                                                 , AccessTypes.DeviceCatalogue
                                                                 , true
                                                                 , cancellationToken);
            item.IsSelectPart = !item.IsSelectFull
                && await _objectAccessBLL.AccessIsGrantedAsync(accessFilter.OwnerID
                                                               , accessFilter.OwnerClassID
                                                               , item.ID
                                                               , item.ClassID
                                                               , AccessTypes.DeviceCatalogue
                                                               , cancellationToken: cancellationToken);
        }
    }
    #endregion
}

