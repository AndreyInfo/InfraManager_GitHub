using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.AssetsManagement.Hardware;

internal class HardwareListAssetSubQuery : IAssetQuery, ISelfRegisteredService<IAssetQuery>
{
    private readonly DbSet<Asset.Asset> _assets;
    private readonly IOwnerQuery _owners;
    private readonly IUtilizerQuery _utilizerQuery;

    public HardwareListAssetSubQuery(
        DbSet<Asset.Asset> assets,
        IOwnerQuery owners,
        IUtilizerQuery utilizerQuery)
    {
        _assets = assets;
        _owners = owners;
        _utilizerQuery = utilizerQuery;
    }

    public IQueryable<AssetListQueryResultItem> Query(Expression<Func<Asset.Asset, bool>> predicate = null)
    {
        return predicate == null
            ? Query(Enumerable.Empty<Expression<Func<Asset.Asset, bool>>>())
            : Query(Enumerable.Repeat(predicate, 1));
    }

    public IQueryable<AssetListQueryResultItem> Query(IEnumerable<Expression<Func<Asset.Asset, bool>>> predicates)
    {
        var assets = _assets
            .AsNoTracking();

        assets = predicates.Aggregate(assets, (current, expression) => current.Where(expression));

        var result =
            from asset in assets
            join owner in _owners.Query()
                on new { ClassID = asset.OwnerClassID, ID = asset.OwnerID, }
                equals new { ClassID = (ObjectClass?) owner.ClassID, ID = (Guid?) owner.ID, }
            into ownerSub
            from owner in ownerSub.DefaultIfEmpty()
            join utilizer in _utilizerQuery.Query()
                on new { ClassID = asset.UtilizerClassID, ID = asset.UtilizerID, }
                equals new { ClassID = (ObjectClass?) utilizer.ClassID, ID = (Guid?)utilizer.ID, }
            into utilizerSub
            from utilizer in utilizerSub.DefaultIfEmpty()
            select new AssetListQueryResultItem
            {
                DeviceID = asset.DeviceID,
                LifeCycleStateID = asset.LifeCycleState.ID,
                LifeCycleStateName = asset.LifeCycleState.Name,
                Agreement = DbFunctions.CastAsString(asset.Agreement),
                UserID = asset.User.IMObjID,
                UserName = User.GetFullName(asset.User.IMObjID),
                Founding = DbFunctions.CastAsString(asset.Founding),
                OwnerID = asset.OwnerID,
                OwnerName = DbFunctions.CastAsString(owner.Name),
                UtilizerID = asset.UtilizerID,
                UtilizerName = DbFunctions.CastAsString(utilizer.Name),
                AppointmentDate = DbFunctions.CastAsDateTime(asset.AppointmentDate),
                Cost = DbFunctions.CastAsDecimal(asset.Cost),
                ServiceCenterID = asset.ServiceCenter.ID,
                ServiceCenterName = DbFunctions.CastAsString(asset.ServiceCenter.Name),
                ServiceContractID = asset.ServiceContract.ID,
                ServiceContractNumber = asset.ServiceContract.Number,
                Warranty = DbFunctions.CastAsDateTime(asset.Warranty),
                SupplierID = asset.Supplier.ID,
                SupplierName = DbFunctions.CastAsString(asset.Supplier.Name),
                DateReceived = DbFunctions.CastAsDateTime(asset.DateReceived),
                DateInquiry = DbFunctions.CastAsDateTime(asset.DateInquiry),
                DateAnnuled = DbFunctions.CastAsDateTime(asset.DateAnnuled),
                UserField1 = DbFunctions.CastAsString(asset.UserField1),
                UserField2 = DbFunctions.CastAsString(asset.UserField2),
                UserField3 = DbFunctions.CastAsString(asset.UserField3),
                UserField4 = DbFunctions.CastAsString(asset.UserField4),
                UserField5 = DbFunctions.CastAsString(asset.UserField5),
                IsWorking = asset.IsWorking,
                ServiceContractLifeCycleStateID = asset.ServiceContract.LifeCycleState.ID,
                ServiceContractLifeCycleStateName = DbFunctions.CastAsString(asset.ServiceContract.LifeCycleState.Name),
                ServiceContractUtcFinishDate = DbFunctions.CastAsDateTime(asset.ServiceContract.UtcFinishDate),
                OnStore = asset.OnStore,
            };

        return result;
    }

    public int[] QueryByOrgStructure(Guid itemID, ObjectClass itemClass, UserTreeSettings.FiltrationFieldEnum field)
    {
        Expression<Func<Asset.Asset, bool>> predicate;

        switch (field)
        {
            case UserTreeSettings.FiltrationFieldEnum.MOL:
                predicate = a =>
                    DbFunctions.ItemInOrganizationItem(
                        itemClass,
                        itemID,
                        ObjectClass.User,
                        a.User.IMObjID);
                break;

            case UserTreeSettings.FiltrationFieldEnum.Owner:
                predicate = a =>
                    DbFunctions.ItemInOrganizationItem(
                        itemClass,
                        itemID,
                        a.OwnerClassID,
                        a.OwnerID);
                break;

            case UserTreeSettings.FiltrationFieldEnum.Utilizer:
                predicate = a =>
                    DbFunctions.ItemInOrganizationItem(
                        itemClass,
                        itemID,
                        a.UtilizerClassID,
                        a.UtilizerID);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(field), field, null);
        }

        return Query(predicate).Select(a => a.DeviceID).ToArray();
    }
}