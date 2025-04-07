using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Inframanager.BLL;
using InfraManager.BLL.AssetsManagement.Hardware;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.AssetsManagement.Hardware;

namespace InfraManager.BLL.AssetsManagement;

internal abstract class AssetSearchListFilterBuilder<TEntity, TModel> : IBuildListViewFilterPredicates<TEntity, AssetSearchListFilter>
    where TEntity : class, IProduct<TModel>, IHardwareAsset
    where TModel : IProductModel
{
    private readonly IValidateObjectPermissions<Guid, TEntity> _permissionValidator;
    private readonly IAssetQuery _assetQuery;
    private readonly ISearchTextPredicateBuilder _searchPredicateBuilder;

    protected AssetSearchListFilterBuilder(
        IValidateObjectPermissions<Guid, TEntity> permissionValidator,
        IAssetQuery assetQuery,
        ISearchTextPredicateBuilder searchPredicateBuilder)
    {
        _permissionValidator = permissionValidator;
        _assetQuery = assetQuery;
        _searchPredicateBuilder = searchPredicateBuilder;
    }

    public IEnumerable<Expression<Func<TEntity, bool>>> Build(Guid userID, AssetSearchListFilter filter)
    {
        var typesIDList = filter.TypesID ?? Array.Empty<Guid>();
        var modelsIDList = filter.ModelsID ?? Array.Empty<Guid>();

        var assetsID = (int[]) null;
        Expression<Func<TEntity, bool>> locationPredicate = null;
        Expression<Func<TEntity, bool>> searchPredicate = null;

        var locationID = filter.LocationID;
        var locationClass = filter.LocationClassID;
        if (locationID.HasValue && locationClass.HasValue)
        {
            locationPredicate = BuildLocationPredicate(locationID.Value, locationClass.Value);
        }

        var orgItemID = filter.OrgStructureObjectID;
        var orgItemClass = filter.OrgStructureObjectClassID;
        var field = filter.OrgStructureFilterType;

        if (orgItemID.HasValue && orgItemClass.HasValue)
        {
            assetsID = _assetQuery.QueryByOrgStructure(orgItemID.Value, orgItemClass.Value, field);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            searchPredicate = _searchPredicateBuilder.BuildPredicate(GetSearchProperties(), filter.SearchText);
        }

        return _permissionValidator.ObjectIsAvailable(userID)
                .UnionIf(typesIDList.Any(), x => typesIDList.Contains(x.Model.ProductCatalogTypeID))
                .UnionIf(modelsIDList.Any(), x => modelsIDList.Contains(x.Model.IMObjID))
                .UnionIf(locationPredicate != null, locationPredicate)
                .UnionIf(searchPredicate != null, searchPredicate)
                .UnionIf(assetsID is not null, x => assetsID.Contains(x.ID));
    }

    protected abstract IEnumerable<Expression<Func<TEntity, string>>> GetSearchProperties();

    protected abstract Expression<Func<TEntity,bool>> BuildLocationPredicate(Guid locationID, ObjectClass locationClass);
}