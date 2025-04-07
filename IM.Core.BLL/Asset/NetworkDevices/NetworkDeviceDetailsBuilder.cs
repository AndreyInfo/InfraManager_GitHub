using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ConfigurationUnits;
using InfraManager.DAL.Location;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.NetworkDevices;

internal sealed class NetworkDeviceDetailsBuilder : IBuildObject<NetworkDeviceDetails, NetworkDevice>
    , ISelfRegisteredService<IBuildObject<NetworkDeviceDetails, NetworkDevice>>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<AssetEntity> _assetRepository;
    private readonly IReadonlyRepository<NetworkNode> _networkNodeRepository;

    public NetworkDeviceDetailsBuilder(IMapper mapper
        , IReadonlyRepository<AssetEntity> assetRepository
        , IReadonlyRepository<NetworkNode> networkNodeRepository)
    {
        _mapper = mapper;
        _assetRepository = assetRepository;
        _networkNodeRepository = networkNodeRepository;
    }

    public async Task<NetworkDeviceDetails> BuildAsync(NetworkDevice entity, CancellationToken cancellationToken = default)
    {
        var details = _mapper.Map<NetworkDeviceDetails>(entity);

        var asset = await _assetRepository.FirstOrDefaultAsync(x => x.DeviceID == entity.ID, cancellationToken);
        var networkNode = await _networkNodeRepository.FirstOrDefaultAsync(x => x.NetworkDeviceID == entity.ID, cancellationToken);

        if (networkNode is not null)
        {
            details.NetworkNodeID = networkNode.ID;
            details.NetworkNodeName = networkNode.Name;
        }

        details.ProductCatalogModelFullName = GetProductCatalogFullName(entity.Model);
        details.Location = GetLocation(entity.Room, entity.Rack);

        _mapper.Map(asset, details);

        return details;
    }

    public Task<IEnumerable<NetworkDeviceDetails>> BuildManyAsync(IEnumerable<NetworkDevice> dataItems, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private string GetProductCatalogFullName(NetworkDeviceModel model)
    {
        return model is null ? model.Name
            : $"{model.ProductCatalogType.ProductCatalogCategory.Name}/ "
            + $"{model.ProductCatalogType.ProductCatalogTemplate.Name}/ "
            + $"{model.Name}";
    }

    private string GetLocation(Room room, Rack rack)
    {
        var roomLocation = room is null ? ""
            : $"{room.Floor.Building.Organization?.Name}/ "
            + $"{room.Floor.Building.Name}/ "
            + $"{room.Floor.Name}/ "
            + $"{room.Name}/ ";

        var rackLocation = rack is null ? "" : $"{rack.Name}";

        return roomLocation + rackLocation;
    }
}
