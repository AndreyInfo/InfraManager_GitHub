using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.AssetsManagement.Hardware;

namespace InfraManager.BLL.AssetsManagement;

internal class AssetSearchListNetworkDeviceFilterBuilder : AssetSearchListFilterBuilder<NetworkDevice, NetworkDeviceModel>
{
    public AssetSearchListNetworkDeviceFilterBuilder(
        IValidateObjectPermissions<Guid, NetworkDevice> permissionValidator,
        IAssetQuery assetQuery,
        ISearchTextPredicateBuilder searchPredicateBuilder)
        : base(permissionValidator, assetQuery, searchPredicateBuilder)
    {
    }

    protected override IEnumerable<Expression<Func<NetworkDevice, string>>> GetSearchProperties()
    {
        yield return x => x.Name;
        yield return x => x.Note;
        yield return x => x.Model.Name;
        yield return x => x.SerialNumber;
        yield return x => x.Code;
        yield return x => x.InvNumber;
    }

    protected override Expression<Func<NetworkDevice, bool>> BuildLocationPredicate(Guid locationID, ObjectClass locationClass)
    {
        switch (locationClass)
        {
            case ObjectClass.Organizaton:
                return x => x.Room.Floor.Building.Organization.ID == locationID;
            case ObjectClass.Building:
                return x => x.Room.Floor.Building.IMObjID == locationID;
            case ObjectClass.Floor:
                return x => x.Room.Floor.IMObjID == locationID;
            case ObjectClass.Room:
                return x => x.Room.IMObjID == locationID;
            default:
                return x => false;
        }
    }
}