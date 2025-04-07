using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Inframanager.BLL;
using InfraManager.BLL.AssetsManagement.Hardware;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.ConfigurationData;

namespace InfraManager.BLL.AssetsManagement;

internal class AssetSearchListDataEntityFilterBuilder : IBuildListViewFilterPredicates<DataEntity, AssetSearchListFilter>
{
    private readonly IValidateObjectPermissions<Guid, DataEntity> _permissionValidator;
    private readonly ISearchTextPredicateBuilder _searchPredicateBuilder;

    public AssetSearchListDataEntityFilterBuilder(
        IValidateObjectPermissions<Guid, DataEntity> permissionValidator,
        ISearchTextPredicateBuilder searchPredicateBuilder)
    {
        _permissionValidator = permissionValidator;
        _searchPredicateBuilder = searchPredicateBuilder;
    }

    public IEnumerable<Expression<Func<DataEntity, bool>>> Build(Guid userID, AssetSearchListFilter filter)
    {
        var typesIDList = filter.TypesID ?? Array.Empty<Guid>();

        Expression<Func<DataEntity, bool>> searchPredicate = null;
        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            searchPredicate = _searchPredicateBuilder.BuildPredicate(GetSearchProperties(), filter.SearchText);
        }

        return _permissionValidator.ObjectIsAvailable(userID)
            .UnionIf(typesIDList.Any(), x => typesIDList.Contains(x.ProductCatalogTypeID))
            .UnionIf(searchPredicate != null, searchPredicate);
    }

    private static IEnumerable<Expression<Func<DataEntity, string>>> GetSearchProperties()
    {
        yield return x => x.Name;
        yield return x => x.Note;
    }
}