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

internal class ClientsHardwareListFilterBuilder<TEntity> : IBuildListViewFilterPredicates<TEntity, ClientsHardwareListFilter>
    where TEntity : class, IHardwareAsset
{
    private readonly IValidateObjectPermissions<Guid, TEntity> _permissionValidator;
    private readonly IAssetQuery _assetQuery;

    public ClientsHardwareListFilterBuilder(IValidateObjectPermissions<Guid, TEntity> permissionValidator, IAssetQuery assetQuery)
    {
        _permissionValidator = permissionValidator;
        _assetQuery = assetQuery;
    }

    public IEnumerable<Expression<Func<TEntity, bool>>> Build(Guid userID, ClientsHardwareListFilter filter)
    {
        var assetsID = _assetQuery.QueryByOrgStructure(filter.ClientID, ObjectClass.User, UserTreeSettings.FiltrationFieldEnum.Utilizer);

        return _permissionValidator.ObjectIsAvailable(userID)
            .Union(x => assetsID.Contains(x.ID));
    }
}