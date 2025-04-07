using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.AssetsManagement.Hardware;

internal class HardwareListPeripheralsSubQuery<TResultQueryItem> :
    IListQuery<Peripheral, TResultQueryItem>
    where TResultQueryItem : HardwareListQueryResultItemBase, new()
{
    private readonly DbSet<Peripheral> _peripherals;
    private readonly IAssetQuery _assets;

    public HardwareListPeripheralsSubQuery(
        DbSet<Peripheral> peripherals,
        IAssetQuery assets)
    {
        _peripherals = peripherals;
        _assets = assets;
    }

    public IQueryable<TResultQueryItem> Query(Guid userId, IEnumerable<Expression<Func<Peripheral, bool>>> predicates)
    {
        var peripherals = _peripherals
            .AsNoTracking()
            .Where(p => p.TerminalDeviceID != 0 || p.NetworkDeviceID != 0 || (p.TerminalDeviceID == 0 && p.NetworkDeviceID == 0))
            .Where(p => p.TerminalDeviceID / 100_000 != 29) // todo: Какая-то логика связанная с опросом оборудования и подтверждением.
            .Where(p => p.Model.ProductCatalogType.IsAccountingAsset == true);

        peripherals = predicates.Aggregate(peripherals, (current, expression) => current.Where(expression));

        return from peripheral in peripherals
            join asset in _assets.Query() on peripheral.ID equals asset.DeviceID into assetSub
            from asset in assetSub.DefaultIfEmpty()
            join td in _assets.Query() on peripheral.TerminalDeviceID equals td.DeviceID into tdSub
            from td in tdSub.DefaultIfEmpty()
            join nd in _assets.Query() on peripheral.NetworkDeviceID equals nd.DeviceID into ndSub
            from nd in ndSub.DefaultIfEmpty()
            select new TResultQueryItem
            {
                ID = peripheral.IMObjID,
                ClassID = ObjectClass.Peripherial,
                Name = null,
                SerialNumber = DbFunctions.CastAsString(peripheral.SerialNumber),
                Code = DbFunctions.CastAsString(peripheral.Code),
                Note = DbFunctions.CastAsString(peripheral.Note),
                TypeName = peripheral.Model.ProductCatalogType.Name,
                ModelName = DbFunctions.CastAsString(peripheral.Model.Name),
                ModelID = peripheral.Model.IMObjID,
                VendorName = DbFunctions.CastAsString(peripheral.Model.Vendor.Name),
                InvNumber = DbFunctions.CastAsString(peripheral.Name),
                ProductCatalogTemplateName = peripheral.Model.ProductCatalogType.ProductCatalogTemplate.Name,
                RackName = DbFunctions.CastAsString(peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID != 0 && peripheral.RoomID == 0 ? peripheral.NetworkDevice.Rack.Name : null),
                RackID = peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID != 0 && peripheral.RoomID == 0 ? peripheral.NetworkDevice.Rack.IMObjID : null,
                WorkplaceName = DbFunctions.CastAsString(peripheral.TerminalDeviceID != 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID == 0 ? peripheral.TerminalDevice.Workplace.Name : null),
                WorkplaceID = peripheral.TerminalDeviceID != 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID == 0 ? peripheral.TerminalDevice.Workplace.IMObjID : null,
                RoomName = DbFunctions.CastAsString(peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID != 0
                    ? peripheral.Room.Name
                    : peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID != 0 && peripheral.RoomID == 0
                        ? peripheral.NetworkDevice.Room.Name
                        : peripheral.TerminalDeviceID != 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID == 0
                            ? peripheral.TerminalDevice.Room.Name
                            : null),
                RoomID = peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID != 0
                    ? peripheral.Room.IMObjID
                    : peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID != 0 && peripheral.RoomID == 0
                        ? peripheral.NetworkDevice.Room.IMObjID
                        : peripheral.TerminalDeviceID != 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID == 0
                            ? peripheral.TerminalDevice.Room.IMObjID
                            : null,
                FloorName = DbFunctions.CastAsString(peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID != 0
                    ? peripheral.Room.Floor.Name
                    : peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID != 0 && peripheral.RoomID == 0
                        ? peripheral.NetworkDevice.Room.Floor.Name
                        : peripheral.TerminalDeviceID != 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID == 0
                            ? peripheral.TerminalDevice.Room.Floor.Name
                            : null),
                BuildingName = DbFunctions.CastAsString(peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID != 0
                    ? peripheral.Room.Floor.Building.Name
                    : peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID != 0 && peripheral.RoomID == 0
                        ? peripheral.NetworkDevice.Room.Floor.Building.Name
                        : peripheral.TerminalDeviceID != 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID == 0
                            ? peripheral.TerminalDevice.Room.Floor.Building.Name
                            : null),
                OrganizationName = DbFunctions.CastAsString(peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID != 0
                    ? peripheral.Room.Floor.Building.Organization.Name
                    : peripheral.TerminalDeviceID == 0 && peripheral.NetworkDeviceID != 0 && peripheral.RoomID == 0
                        ? peripheral.NetworkDevice.Room.Floor.Building.Organization.Name
                        : peripheral.TerminalDeviceID != 0 && peripheral.NetworkDeviceID == 0 && peripheral.RoomID == 0
                            ? peripheral.TerminalDevice.Room.Floor.Building.Organization.Name
                            : null),
                LocationOnStore = peripheral.TerminalDeviceID != 0
                    ? td.OnStore
                    : peripheral.NetworkDeviceID != 0
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
                FullObjectName = DbFunctions.GetFullObjectName(ObjectClass.Peripherial, peripheral.IMObjID),
                FullObjectLocation = DbFunctions.GetFullObjectLocation(ObjectClass.Peripherial, peripheral.IMObjID),
                IPAddress = null,
                ConfigurationUnitName = null,
            };
    }
}