using InfraManager.DAL.Asset.Peripherals;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.DAL.Asset;
internal class PeripheralQuery : IPeripheralQuery, ISelfRegisteredService<IPeripheralQuery>
{
    private readonly DbSet<AssetEntity> _assetDbSet;
    private readonly IOwnerQuery _owners;
    private readonly IUtilizerQuery _utilizerQuery;

    public PeripheralQuery(
        DbSet<AssetEntity> assetDbSet,
        IOwnerQuery owners,
        IUtilizerQuery utilizerQuery
        )
    {
        _assetDbSet = assetDbSet;
        _owners = owners;
        _utilizerQuery = utilizerQuery;
    }

    public async Task<PeripheralItem[]> ExecuteAsync(PaggingFilter filter, IOrderedQueryable<Peripheral> orderedQuery, CancellationToken cancellationToken)
    {
        var query = orderedQuery.Select(x => new PeripheralItem
        {
            ProductCatalogTypeName = x.Model.ProductCatalogType.Name,
            ProductCatalogModelName = x.Model.Name,
            TerminalDeviceName = x.TerminalDevice.Name,
            NetworkDeviceName = x.NetworkDevice.Name,
            SerialNumber = x.SerialNumber,
            Code = x.Code,
            Parameters = x.Model.Parameters,
            BWLoad = x.BWLoad,
            ColorLoad = x.ColorLoad,
            PhotoLoad = x.PhotoLoad,
            Note = x.Note,
            InquiryState = null, // TODO: После реализации опроса сети
            ProductCatalogTemplateName = x.Model.ProductCatalogType.ProductCatalogTemplate.Name,
            LifeCycleStateName = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).LifeCycleState.Name,
            IsWorking = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).IsWorking,
            Cost = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).Cost,
            Founding = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).Founding,
            AppointmentDate = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).AppointmentDate,
            UserName = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).User.Name,
            SupplierName = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).Supplier.Name,
            DateReceived = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).DateReceived,
            DateAnnuled = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).DateAnnuled,
            Agreement = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).Agreement,
            Warranty = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).Warranty,
            ServiceCenterName = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).ServiceCenter.Name,
            ServiceContractNumber = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).ServiceContract.Number,
            OwnerName = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).OwnerID.HasValue
                ? _owners.Query().FirstOrDefault(
                    z => z.ID == _assetDbSet.AsNoTracking().FirstOrDefault(
                    y => y.DeviceID == x.ID).OwnerID).Name
                : "",
            UtilizerName = _assetDbSet.AsNoTracking().FirstOrDefault(y => y.DeviceID == x.ID).OwnerID.HasValue
                ? _utilizerQuery.Query().FirstOrDefault(
                    z => z.ID == _assetDbSet.AsNoTracking().FirstOrDefault(
                    y => y.DeviceID == x.ID).UtilizerID).Name
                : "",
            ExternalID = x.ExternalID
        });

        if (!string.IsNullOrEmpty(filter.SearchString))
            query = query.Where(x => x.ProductCatalogModelName.ToLower().Contains(filter.SearchString.ToLower()));

        if (filter.Take > 0)
        {
            query = query.Skip(filter.Skip).Take(filter.Take);
        }

        return await query.ToArrayAsync(cancellationToken);
    }
}
