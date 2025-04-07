using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.AssetsManagement.Hardware;

internal class HardwareModelNameLookupQuery : ILookupQuery
{
    private readonly DbSet<NetworkDeviceModel> _networkDeviceModels;
    private readonly DbSet<TerminalDeviceModel> _terminalDeviceModels;
    private readonly DbSet<AdapterType> _adapterTypes;
    private readonly DbSet<PeripheralType> _peripheralTypes;

    public HardwareModelNameLookupQuery(
        DbSet<NetworkDeviceModel> networkDeviceModels,
        DbSet<TerminalDeviceModel> terminalDeviceModels,
        DbSet<AdapterType> adapterTypes,
        DbSet<PeripheralType> peripheralTypes)
    {
        _networkDeviceModels = networkDeviceModels;
        _terminalDeviceModels = terminalDeviceModels;
        _adapterTypes = adapterTypes;
        _peripheralTypes = peripheralTypes;
    }

    public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var adapterTypes = _adapterTypes
            .Include(x => x.ProductCatalogType)
            .AsNoTracking()
            .Select(x => new
            {
                ID = x.IMObjID,
                ProductCatalogTypeName = x.ProductCatalogType.Name,
                TypeName = DbFunctions.CastAsString((string) x.Name),
            });

        var peripheralTypes = _peripheralTypes
            .Include(x => x.ProductCatalogType)
            .AsNoTracking()
            .Select(x => new
            {
                ID = x.IMObjID,
                ProductCatalogTypeName = x.ProductCatalogType.Name,
                TypeName = DbFunctions.CastAsString(x.Name),
            });

        var networkDeviceModels = _networkDeviceModels
            .Include(x => x.ProductCatalogType)
            .AsNoTracking()
            .Select(x => new
            {
                ID = x.IMObjID,
                ProductCatalogTypeName = x.ProductCatalogType.Name,
                TypeName = DbFunctions.CastAsString(x.Name),
            });

        var terminalDeviceModels = _terminalDeviceModels
            .Include(x => x.ProductCatalogType)
            .AsNoTracking()
            .Select(x => new
            {
                ID = x.IMObjID,
                ProductCatalogTypeName = x.ProductCatalogType.Name,
                TypeName = DbFunctions.CastAsString(x.Name),
            });

        return Array.ConvertAll(await adapterTypes
                .Concat(peripheralTypes)
                .Concat(networkDeviceModels)
                .Concat(terminalDeviceModels)
                .Distinct()
                .ToArrayAsync(cancellationToken),
            item => new ValueData
            {
                ID = item.ID.ToString(),
                Info = $"{item.ProductCatalogTypeName} \\ {item.TypeName}",
            });
    }
}