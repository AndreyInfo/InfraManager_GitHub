using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.AssetsManagement.Hardware;

internal class HardwareListAdaptersSubQuery<TResultQueryItem> :
    IListQuery<Adapter, TResultQueryItem>
    where TResultQueryItem : HardwareListQueryResultItemBase, new()
{
    private readonly DbSet<Adapter> _adapters;
    private readonly IAssetQuery _assets;

    public HardwareListAdaptersSubQuery(
        DbSet<Adapter> adapters,
        IAssetQuery assets)
    {
        _adapters = adapters;
        _assets = assets;
    }

    public IQueryable<TResultQueryItem> Query(Guid userId, IEnumerable<Expression<Func<Adapter, bool>>> predicates)
    {
        var adapters = _adapters
            .AsNoTracking()
            .Where(a => a.TerminalDeviceID != 0 || a.NetworkDeviceID != 0 || (a.TerminalDeviceID == 0 && a.NetworkDeviceID == 0))
            .Where(a => a.TerminalDeviceID / 100_000 != 29) // todo: Какая-то логика связанная с опросом оборудования и подтверждением.
            .Where(a => !a.Integrated)
            .Where(a => a.Model.ProductCatalogType.IsAccountingAsset == true);

        adapters = predicates.Aggregate(adapters, (current, expression) => current.Where(expression));

        return from adapter in adapters
            join asset in _assets.Query() on adapter.ID equals asset.DeviceID into assetSub
            from asset in assetSub.DefaultIfEmpty()
            join td in _assets.Query() on adapter.TerminalDeviceID equals td.DeviceID into tdSub
            from td in tdSub.DefaultIfEmpty()
            join nd in _assets.Query() on adapter.NetworkDeviceID equals nd.DeviceID into ndSub
            from nd in ndSub.DefaultIfEmpty()
            select new TResultQueryItem
            {
                ID = adapter.IMObjID,
                ClassID = ObjectClass.Adapter,
                Name = null,
                SerialNumber = DbFunctions.CastAsString(adapter.SerialNumber),
                Code = DbFunctions.CastAsString(adapter.Code),
                Note = DbFunctions.CastAsString(adapter.Note),
                TypeName = adapter.Model.ProductCatalogType.Name,
                ModelName = DbFunctions.CastAsString(adapter.Model.Name),
                ModelID = adapter.Model.IMObjID,
                VendorName = DbFunctions.CastAsString(adapter.Model.Vendor.Name),
                InvNumber = DbFunctions.CastAsString(adapter.Name),
                ProductCatalogTemplateName = adapter.Model.ProductCatalogType.ProductCatalogTemplate.Name,
                RoomName = DbFunctions.CastAsString(adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID != 0
                    ? adapter.Room.Name
                    : adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID != 0 && adapter.RoomID == 0
                        ? adapter.NetworkDevice.Room.Name
                        : adapter.TerminalDeviceID != 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID == 0
                            ? adapter.TerminalDevice.Room.Name
                            : null),
                RoomID = adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID != 0
                    ? adapter.Room.IMObjID
                    : adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID != 0 && adapter.RoomID == 0
                        ? adapter.NetworkDevice.Room.IMObjID
                        : adapter.TerminalDeviceID != 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID == 0
                            ? adapter.TerminalDevice.Room.IMObjID
                            : null,
                RackName = DbFunctions.CastAsString(adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID != 0 && adapter.RoomID == 0 ? adapter.NetworkDevice.Rack.Name : null),
                RackID = adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID != 0 && adapter.RoomID == 0 ? adapter.NetworkDevice.Rack.IMObjID : null,
                WorkplaceName = DbFunctions.CastAsString(adapter.TerminalDeviceID != 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID == 0 ? adapter.TerminalDevice.Workplace.Name : null),
                WorkplaceID = adapter.TerminalDeviceID != 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID == 0 ? adapter.TerminalDevice.Workplace.IMObjID : null,
                FloorName = DbFunctions.CastAsString(adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID != 0
                    ? adapter.Room.Floor.Name
                    : adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID != 0 && adapter.RoomID == 0
                        ? adapter.NetworkDevice.Room.Floor.Name
                        : adapter.TerminalDeviceID != 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID == 0
                            ? adapter.TerminalDevice.Room.Floor.Name
                            : null),
                BuildingName = DbFunctions.CastAsString(adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID != 0
                    ? adapter.Room.Floor.Building.Name
                    : adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID != 0 && adapter.RoomID == 0
                        ? adapter.NetworkDevice.Room.Floor.Building.Name
                        : adapter.TerminalDeviceID != 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID == 0
                            ? adapter.TerminalDevice.Room.Floor.Building.Name
                            : null),
                OrganizationName = DbFunctions.CastAsString(adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID != 0
                    ? adapter.Room.Floor.Building.Organization.Name
                    : adapter.TerminalDeviceID == 0 && adapter.NetworkDeviceID != 0 && adapter.RoomID == 0
                        ? adapter.NetworkDevice.Room.Floor.Building.Organization.Name
                        : adapter.TerminalDeviceID != 0 && adapter.NetworkDeviceID == 0 && adapter.RoomID == 0
                            ? adapter.TerminalDevice.Room.Floor.Building.Organization.Name
                            : null),
                LocationOnStore = adapter.TerminalDeviceID != 0
                    ? td.OnStore
                    : adapter.NetworkDeviceID != 0
                        ? nd.OnStore
                        : true,
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
                FullObjectName = DbFunctions.GetFullObjectName(ObjectClass.Adapter, adapter.IMObjID),
                FullObjectLocation = DbFunctions.GetFullObjectLocation(ObjectClass.Adapter, adapter.IMObjID),
                IPAddress = null,
                ConfigurationUnitName = null,
            };
    }
}