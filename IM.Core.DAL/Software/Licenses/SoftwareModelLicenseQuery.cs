using InfraManager.DAL.Asset;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;


namespace InfraManager.DAL.Software.Licenses;
internal class SoftwareModelLicenseQuery : ISoftwareModelLicenseQuery, ISelfRegisteredService<ISoftwareModelLicenseQuery>
{
    private readonly DbSet<SoftwareLicenceScheme> _licenceSchemeDbSet;
    private readonly DbSet<Manufacturer> _manufacturerDbSet;
    private readonly DbSet<ServiceContractLicence> _serviceContractLicenseDbSet;
    private readonly DbSet<ServiceContract> _serviceContractDbSet;
    private readonly DbSet<AssetEntity> _assetDbSet;
    private readonly DbSet<User> _userDbSet;
    private readonly DbSet<Room> _roomDbSet;

    public SoftwareModelLicenseQuery(
        DbSet<SoftwareLicenceScheme> licenceSchemeDbSet,
        DbSet<Manufacturer> manufacturerDbSet,
        DbSet<ServiceContractLicence> serviceContractLicenseDbSet,
        DbSet<ServiceContract> serviceContractDbSet,
        DbSet<AssetEntity> assetDbSet,
        DbSet<User> userDbSet,
        DbSet<Room> roomDbSet
        )
    {
        _manufacturerDbSet = manufacturerDbSet;
        _licenceSchemeDbSet = licenceSchemeDbSet;
        _serviceContractLicenseDbSet = serviceContractLicenseDbSet;
        _serviceContractDbSet = serviceContractDbSet;
        _assetDbSet = assetDbSet;
        _userDbSet = userDbSet;
        _roomDbSet = roomDbSet;
    }

    public async Task<SoftwareModelLicenseItem[]> ExecuteAsync(PaggingFilter filter, IOrderedQueryable<SoftwareLicence> orderedQuery, CancellationToken cancellationToken)
    {
        var query = orderedQuery.Select(x => new SoftwareModelLicenseItem
        {
            ID = x.ID,
            Name = x.Name,
            Note = x.Note,
            EndDate = x.EndDate,
            BeginDate = x.BeginDate,
            RoomIntID = x.RoomIntID,
            InventoryNumber = x.InventoryNumber,
            SoftwareModelID = x.SoftwareModelID,
            SoftwareModelName = x.SoftwareModel.Name,
            SoftwareModelVersion = x.SoftwareModel.Version,
            LicenceType = (LicenceType)x.SoftwareLicenceType,
            SoftwareLicenseSchemeID = x.SoftwareLicenceScheme,
            ProductCatalogTypeName = x.ProductCatalogType.Name,
            SoftwareTypeName = x.SoftwareModel.SoftwareType.Name,
            SoftwareLicenceSchemeEnum = x.SoftwareLicenceSchemeEnum,

            RoomIMObjID = _roomDbSet.AsNoTracking().FirstOrDefault(y => y.ID == x.RoomIntID).IMObjID,

            Count = _serviceContractLicenseDbSet.AsNoTracking().FirstOrDefault(y => y.SoftwareModelID == x.SoftwareModelID).Count,

            SoftwareLicenseSchemeName = _licenceSchemeDbSet.AsNoTracking().FirstOrDefault(y => y.ID == x.SoftwareLicenceScheme).Name,

            ManufacturerName = _manufacturerDbSet.AsNoTracking().FirstOrDefault(y => y.ImObjID == x.SoftwareModel.ManufacturerID).Name,

            ManufacturerID = x.SoftwareModel.ManufacturerID
                ?? _manufacturerDbSet.AsNoTracking().FirstOrDefault(x => x.ID == Manufacturer.EmptyID).ImObjID.Value,

            LimitInDays = _serviceContractLicenseDbSet.AsNoTracking()
                .FirstOrDefault(y => y.SoftwareModelID == x.SoftwareModelID).LimitInDays,

            ServiceContractID = _serviceContractLicenseDbSet.AsNoTracking()
                .FirstOrDefault(y => y.SoftwareModelID == x.SoftwareModelID).ServiceContractID,

            UtcFinishDate = _serviceContractDbSet.AsNoTracking().FirstOrDefault(
                z => z.ID == _serviceContractLicenseDbSet.AsNoTracking().FirstOrDefault(
                    y => y.SoftwareModelID == x.SoftwareModelID).ServiceContractID).UtcFinishDate,

            Warranty = _assetDbSet.AsNoTracking().FirstOrDefault(
                y => y.ServiceContractID == _serviceContractLicenseDbSet.AsNoTracking().FirstOrDefault(
                    y => y.SoftwareModelID == x.SoftwareModelID).ServiceContractID).Warranty,

            Supplier = _serviceContractDbSet.AsNoTracking().FirstOrDefault(
                z => z.ID == _serviceContractLicenseDbSet.AsNoTracking().FirstOrDefault(
                    y => y.SoftwareModelID == x.SoftwareModelID).ServiceContractID).Supplier.Name,

            UserName = _assetDbSet.AsNoTracking().FirstOrDefault(
                y => y.ServiceContractID == _serviceContractLicenseDbSet.AsNoTracking().FirstOrDefault(
                    y => y.SoftwareModelID == x.SoftwareModelID).ServiceContractID).User.Name,

            OwnerName = _userDbSet.AsNoTracking().FirstOrDefault(
                z => z.IMObjID == _assetDbSet.AsNoTracking().FirstOrDefault(
                    y => y.ServiceContractID == _serviceContractLicenseDbSet.AsNoTracking().FirstOrDefault(
                        y => y.SoftwareModelID == x.SoftwareModelID).ServiceContractID).OwnerID).Name,

            UtilizerName = _userDbSet.AsNoTracking().FirstOrDefault(
                z => z.IMObjID == _assetDbSet.AsNoTracking().FirstOrDefault(
                    y => y.ServiceContractID == _serviceContractLicenseDbSet.AsNoTracking().FirstOrDefault(
                        y => y.SoftwareModelID == x.SoftwareModelID).ServiceContractID).UtilizerID).Name,

            LifeCycleStateName = _assetDbSet.AsNoTracking().FirstOrDefault(
                y => y.ServiceContractID == _serviceContractLicenseDbSet.AsNoTracking().FirstOrDefault(
                    y => y.SoftwareModelID == x.SoftwareModelID).ServiceContractID).LifeCycleState.Name,

            Cost = _assetDbSet.AsNoTracking().FirstOrDefault(
                y => y.ServiceContractID == _serviceContractLicenseDbSet.AsNoTracking().FirstOrDefault(
                    y => y.SoftwareModelID == x.SoftwareModelID).ServiceContractID).Cost,


        });

        if (!string.IsNullOrEmpty(filter.SearchString))
            query = query.Where(c => c.Name.ToLower().Contains(filter.SearchString.ToLower()));

        if (filter.Take > 0)
        {
            query = query.Skip(filter.Skip).Take(filter.Take);
        }

        return await query.ToArrayAsync(cancellationToken);
    }
}
