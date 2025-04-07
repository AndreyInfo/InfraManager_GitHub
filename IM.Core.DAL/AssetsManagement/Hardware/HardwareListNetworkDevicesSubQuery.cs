using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.AssetsManagement.Hardware;

internal class HardwareListNetworkDevicesSubQuery<TResultQueryItem> :
    IListQuery<NetworkDevice, TResultQueryItem>
    where TResultQueryItem : HardwareListQueryResultItemBase, new()
{
    private readonly DbSet<NetworkDevice> _networkDevices;
    private readonly IAssetQuery _assets;

    public HardwareListNetworkDevicesSubQuery(
        DbSet<NetworkDevice> networkDevices,
        IAssetQuery assets)
    {
        _networkDevices = networkDevices;
        _assets = assets;
    }

    public IQueryable<TResultQueryItem> Query(Guid userId, IEnumerable<Expression<Func<NetworkDevice, bool>>> predicates)
    {
        var networkDevices = _networkDevices
            .AsNoTracking()
            .Where(nd => nd.ID != 0)
            .Where(nd => nd.Model.ProductCatalogType.IsLogical != true)
            .Where(nd => nd.Model.ProductCatalogType.IsAccountingAsset == true);

        networkDevices = predicates.Aggregate(networkDevices, (current, expression) => current.Where(expression));

        return from networkDevice in networkDevices
            join asset in _assets.Query() on networkDevice.ID equals asset.DeviceID into assetSub
            from asset in assetSub.DefaultIfEmpty()
            select new TResultQueryItem
            {
                ID = networkDevice.IMObjID,
                ClassID = ObjectClass.ActiveDevice,
                Name = DbFunctions.CastAsString(networkDevice.Name),
                SerialNumber = DbFunctions.CastAsString(networkDevice.SerialNumber),
                Code = DbFunctions.CastAsString(networkDevice.Code),
                Note = DbFunctions.CastAsString(networkDevice.Note),
                TypeName = networkDevice.Model.ProductCatalogType.Name,
                ModelName = DbFunctions.CastAsString(networkDevice.Model.Name),
                ModelID = networkDevice.Model.IMObjID,
                VendorName = DbFunctions.CastAsString(networkDevice.Model.Manufacturer.Name),
                InvNumber = DbFunctions.CastAsString(networkDevice.InvNumber),
                ProductCatalogTemplateName = networkDevice.Model.ProductCatalogType.ProductCatalogTemplate.Name,
                RoomName = DbFunctions.CastAsString(networkDevice.RoomID != 0 ? networkDevice.Room.Name : null),
                RoomID = networkDevice.RoomID != 0 ? networkDevice.Room.IMObjID : null,
                RackName = DbFunctions.CastAsString(networkDevice.Rack.Name),
                RackID = networkDevice.Rack.IMObjID,
                WorkplaceName = null,
                WorkplaceID = null,
                FloorName = DbFunctions.CastAsString(networkDevice.RoomID != 0 ? networkDevice.Room.Floor.Name : null),
                BuildingName = DbFunctions.CastAsString(networkDevice.RoomID != 0 ? networkDevice.Room.Floor.Building.Name : null),
                OrganizationName = DbFunctions.CastAsString(networkDevice.RoomID != 0 ? networkDevice.Room.Floor.Building.Organization.Name : null),
                LocationOnStore = asset.OnStore,
                AssetItem = asset,
                LifeCycleStateName = asset.LifeCycleStateName,
                LifeCycleStateID = asset.LifeCycleStateID,
                Agreement = asset.Agreement,
                UserName = asset.UserName,
                Founding = asset.Founding,
                OwnerName = asset.OwnerName,
                UtilizerName = asset.UtilizerName,
                AppointmentDate = asset.AppointmentDate,
                Cost = asset.Cost,
                ServiceCenterName = asset.ServiceCenterName,
                ServiceContractNumber = asset.ServiceContractNumber,
                Warranty = asset.Warranty,
                SupplierName = asset.SupplierName,
                DateReceived = asset.DateReceived,
                DateInquiry = asset.DateInquiry,
                DateAnnuled = asset.DateAnnuled,
                IsWorking = asset.IsWorking,
                ServiceContractLifeCycleStateName = asset.ServiceContractLifeCycleStateName,
                ServiceContractUtcFinishDate = asset.ServiceContractUtcFinishDate,
                FullObjectName = DbFunctions.GetFullObjectName(ObjectClass.ActiveDevice, networkDevice.IMObjID),
                FullObjectLocation = DbFunctions.GetFullObjectLocation(ObjectClass.ActiveDevice, networkDevice.IMObjID),
                IPAddress = null,
                ConfigurationUnitName = null,
            };
    }
}