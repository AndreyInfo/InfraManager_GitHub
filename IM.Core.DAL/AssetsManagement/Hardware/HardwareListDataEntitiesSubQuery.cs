using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.ConfigurationData;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.AssetsManagement.Hardware;

internal class HardwareListDataEntitiesSubQuery :
    IListQuery<DataEntity, AssetSearchListQueryResultItem>
{
    private readonly DbSet<DataEntity> _dataEntities;

    public HardwareListDataEntitiesSubQuery(DbSet<DataEntity> dataEntities)
    {
        _dataEntities = dataEntities;
    }

    public IQueryable<AssetSearchListQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<DataEntity, bool>>> predicates)
    {
        var dataEntities = _dataEntities
            .AsNoTracking();

        dataEntities = predicates.Aggregate(dataEntities, (current, expression) => current.Where(expression));

        return from de in dataEntities
            select new AssetSearchListQueryResultItem
            {
                ID = de.ID,
                ClassID = ObjectClass.DataEntity,
                Name = DbFunctions.CastAsString(de.Name),
                SerialNumber = null,
                Code = null,
                Note = DbFunctions.CastAsString(de.Note),
                TypeName = de.Type.Name,
                ModelName = null,
                ModelID = Guid.Empty,
                VendorName = null,
                InvNumber = null,
                ProductCatalogTemplateName = de.Type.ProductCatalogTemplate.Name,
                RoomName = null,
                RoomID = null,
                RackName = null,
                RackID = null,
                WorkplaceName = null,
                WorkplaceID = null,
                FloorName = null,
                BuildingName = null,
                OrganizationName = null,
                LocationOnStore = false,
                AssetItem = new AssetListQueryResultItem
                {
                    DeviceID = Asset.Asset.NonExistentDeviceID,
                    LifeCycleStateID = de.LifeCycleState.ID,
                    LifeCycleStateName = de.LifeCycleState.Name,
                    Agreement = null,
                    UserID = null,
                    UserName = null,
                    Founding = null,
                    OwnerID = null,
                    OwnerName = null,
                    UtilizerID = null,
                    UtilizerName = null,
                    AppointmentDate = DbFunctions.CastAsDateTime(null),
                    Cost = DbFunctions.CastAsDecimal(null),
                    ServiceCenterID = null,
                    ServiceCenterName = null,
                    ServiceContractID = null,
                    ServiceContractNumber = null,
                    Warranty = DbFunctions.CastAsDateTime(null),
                    SupplierID = null,
                    SupplierName = null,
                    DateReceived = DbFunctions.CastAsDateTime(null),
                    DateInquiry = DbFunctions.CastAsDateTime(null),
                    DateAnnuled = DbFunctions.CastAsDateTime(null),
                    UserField1 = null,
                    UserField2 = null,
                    UserField3 = null,
                    UserField4 = null,
                    UserField5 = null,
                    IsWorking = false,
                    ServiceContractLifeCycleStateID = null,
                    ServiceContractLifeCycleStateName = null,
                    ServiceContractUtcFinishDate = DbFunctions.CastAsDateTime(null),
                    OnStore = false,
                },
                LifeCycleStateName = de.LifeCycleState.Name,
                Agreement = null,
                UserName = null,
                Founding = null,
                OwnerName = null,
                UtilizerName = null,
                AppointmentDate = DbFunctions.CastAsDateTime(null),
                Cost = DbFunctions.CastAsDecimal(null),
                ServiceCenterName = null,
                ServiceContractNumber = null,
                Warranty = DbFunctions.CastAsDateTime(null),
                SupplierName = null,
                DateReceived = DbFunctions.CastAsDateTime(null),
                DateInquiry = DbFunctions.CastAsDateTime(null),
                DateAnnuled = DbFunctions.CastAsDateTime(null),
                IsWorking = false,
                ServiceContractLifeCycleStateName = null,
                ServiceContractUtcFinishDate = DbFunctions.CastAsDateTime(null),
                FullObjectName = DbFunctions.GetFullObjectName(ObjectClass.DataEntity, de.ID),
                FullObjectLocation = DbFunctions.GetFullObjectLocation(ObjectClass.DataEntity, de.ID),
                IPAddress = null,
                ConfigurationUnitName = null,
            };
    }
}