using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.AssetsManagement.Hardware;

internal class HardwareListTerminalDevicesSubQuery<TResultQueryItem> :
    IListQuery<TerminalDevice, TResultQueryItem>
    where TResultQueryItem : HardwareListQueryResultItemBase, new()
{
    private readonly DbSet<TerminalDevice> _terminalDevices;
    private readonly IAssetQuery _assets;

    public HardwareListTerminalDevicesSubQuery(
        DbSet<TerminalDevice> terminalDevices,
        IAssetQuery assets)
    {
        _terminalDevices = terminalDevices;
        _assets = assets;
    }

    public IQueryable<TResultQueryItem> Query(Guid userId, IEnumerable<Expression<Func<TerminalDevice, bool>>> predicates)
    {
        var terminalDevices = _terminalDevices
                .AsNoTracking()
                .Where(td => td.ID != 0)
                .Where(td => td.Model.ProductCatalogType.IsAccountingAsset == true);
        
        terminalDevices = predicates.Aggregate(terminalDevices, (current, expression) => current.Where(expression));

        return from terminalDevice in terminalDevices
            join asset in _assets.Query() on terminalDevice.ID equals asset.DeviceID into assetSub
            from asset in assetSub.DefaultIfEmpty()
            select new TResultQueryItem
            {
                ID = terminalDevice.IMObjID,
                ClassID = ObjectClass.TerminalDevice,
                Name = DbFunctions.CastAsString(terminalDevice.Name),
                SerialNumber = DbFunctions.CastAsString(terminalDevice.SerialNumber),
                Code = DbFunctions.CastAsString(terminalDevice.Code),
                Note = DbFunctions.CastAsString(terminalDevice.Note),
                TypeName = terminalDevice.Model.ProductCatalogType.Name,
                ModelName = DbFunctions.CastAsString(terminalDevice.Model.Name),
                ModelID = terminalDevice.Model.IMObjID,
                VendorName = DbFunctions.CastAsString(terminalDevice.Model.Manufacturer.Name),
                InvNumber = DbFunctions.CastAsString(terminalDevice.InvNumber),
                ProductCatalogTemplateName = terminalDevice.Model.ProductCatalogType.ProductCatalogTemplate.Name,
                RoomName = DbFunctions.CastAsString(terminalDevice.RoomID != 0 ? terminalDevice.Room.Name : null),
                RoomID = terminalDevice.RoomID != 0 ? terminalDevice.Room.IMObjID : null,
                RackName = null,
                RackID = null,
                WorkplaceName = DbFunctions.CastAsString(terminalDevice.Workplace.Name),
                WorkplaceID = terminalDevice.Workplace.IMObjID,
                FloorName = DbFunctions.CastAsString(terminalDevice.RoomID != 0 ? terminalDevice.Room.Floor.Name : null),
                BuildingName = DbFunctions.CastAsString(terminalDevice.RoomID != 0 ? terminalDevice.Room.Floor.Building.Name : null),
                OrganizationName = DbFunctions.CastAsString(terminalDevice.RoomID != 0 ? terminalDevice.Room.Floor.Building.Organization.Name : null),
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
                FullObjectName = DbFunctions.GetFullObjectName(ObjectClass.TerminalDevice, terminalDevice.IMObjID),
                FullObjectLocation = DbFunctions.GetFullObjectLocation(ObjectClass.TerminalDevice, terminalDevice.IMObjID),
                IPAddress = null,
                ConfigurationUnitName = null,
            };
    }
}