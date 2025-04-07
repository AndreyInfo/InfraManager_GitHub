using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.AssetsManagement.Hardware;

namespace InfraManager.BLL.AssetsManagement;

internal class AssetSearchListAdapterFilterBuilder : AssetSearchListFilterBuilder<Adapter, AdapterType>
{
    public AssetSearchListAdapterFilterBuilder(
        IValidateObjectPermissions<Guid, Adapter> permissionValidator,
        IAssetQuery assetQuery,
        ISearchTextPredicateBuilder searchPredicateBuilder)
        : base(permissionValidator, assetQuery, searchPredicateBuilder)
    {
    }

    protected override Expression<Func<Adapter, bool>> BuildLocationPredicate(Guid locationID, ObjectClass locationClass)
    {
        switch (locationClass)
        {
            case ObjectClass.Organizaton:
                return x => x.TerminalDeviceID == 0 && x.NetworkDeviceID == 0 && x.RoomID != 0 && x.Room.Floor.Building.Organization.ID == locationID
                            || x.TerminalDeviceID == 0 && x.NetworkDeviceID != 0 && x.RoomID == 0 && x.NetworkDevice.Room.Floor.Building.Organization.ID == locationID
                            || x.TerminalDeviceID != 0 && x.NetworkDeviceID == 0 && x.RoomID == 0 && x.TerminalDevice.Room.Floor.Building.Organization.ID == locationID;

            case ObjectClass.Building:
                return x => x.TerminalDeviceID == 0 && x.NetworkDeviceID == 0 && x.RoomID != 0 && x.Room.Floor.Building.IMObjID == locationID
                            || x.TerminalDeviceID == 0 && x.NetworkDeviceID != 0 && x.RoomID == 0 && x.NetworkDevice.Room.Floor.Building.IMObjID == locationID
                            || x.TerminalDeviceID != 0 && x.NetworkDeviceID == 0 && x.RoomID == 0 && x.TerminalDevice.Room.Floor.Building.IMObjID == locationID;

            case ObjectClass.Floor:
                return x => x.TerminalDeviceID == 0 && x.NetworkDeviceID == 0 && x.RoomID != 0 && x.Room.Floor.IMObjID == locationID
                            || x.TerminalDeviceID == 0 && x.NetworkDeviceID != 0 && x.RoomID == 0 && x.NetworkDevice.Room.Floor.IMObjID == locationID
                            || x.TerminalDeviceID != 0 && x.NetworkDeviceID == 0 && x.RoomID == 0 && x.TerminalDevice.Room.Floor.IMObjID == locationID;

            case ObjectClass.Room:
                return x => x.TerminalDeviceID == 0 && x.NetworkDeviceID == 0 && x.RoomID != 0 && x.Room.IMObjID == locationID
                            || x.TerminalDeviceID == 0 && x.NetworkDeviceID != 0 && x.RoomID == 0 && x.NetworkDevice.Room.IMObjID == locationID
                            || x.TerminalDeviceID != 0 && x.NetworkDeviceID == 0 && x.RoomID == 0 && x.TerminalDevice.Room.IMObjID == locationID;
            
            case ObjectClass.Workplace:
                return x => x.TerminalDeviceID != 0 && x.NetworkDeviceID == 0 && x.RoomID == 0 && x.TerminalDevice.Workplace.IMObjID == locationID;
            
            default:
                return x => false;
        }
    }

    protected override IEnumerable<Expression<Func<Adapter, string>>> GetSearchProperties()
    {
        yield return x => x.Name;
        yield return x => x.Note;
        yield return x => x.Model.Name;
        yield return x => x.SerialNumber;
        yield return x => x.Code;
    }
}