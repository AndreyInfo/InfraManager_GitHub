using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.Adapters;

internal sealed class AdapterDetailsBuilder : IBuildObject<AdapterDetails, Adapter>
    , ISelfRegisteredService<IBuildObject<AdapterDetails, Adapter>>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<AssetEntity> _assetRepository;
    private readonly IRepository<NetworkDevice> _networkDeviceRepository;

    public AdapterDetailsBuilder(
          IMapper mapper
        , IReadonlyRepository<AssetEntity> assetRepository
        , IRepository<NetworkDevice> networkDeviceRepository
        )
    {
        _mapper = mapper;
        _assetRepository = assetRepository;
        _networkDeviceRepository = networkDeviceRepository;
    }

    public async Task<AdapterDetails> BuildAsync(Adapter entity, CancellationToken cancellationToken = default)
    {
        var adapter = _mapper.Map<AdapterDetails>(entity);

        var asset = await _assetRepository.With(x=>x.User).ThenWith(x=>x.Workplace)
            .With(x=>x.Owner).With(x=>x.Utilizer)
            .FirstOrDefaultAsync(x => x.ID == adapter.IMObjID, cancellationToken);
        _mapper.Map(asset, adapter);

        adapter.Owner = asset?.Owner?.FullName;
        adapter.Utilizer = asset?.Utilizer?.FullName;
        adapter.Location = GetLocation(adapter, asset?.User, cancellationToken);

        var workplace = asset?.User?.Workplace;
        adapter.WorkplaceName = workplace?.Name;
        adapter.WorkplaceID = workplace?.ID;
        
        var rack = await GetRackAsync(adapter, cancellationToken);
        adapter.RackName = rack?.Name;
        adapter.RackID = rack?.ID;

        return adapter;
    }

    private string GetLocation(AdapterDetails adapter, User user, CancellationToken cancellationToken)
    {
        string sep = "/";
        return string.Concat(adapter.OrganizationName, sep, adapter.BuildingName, sep,
            adapter.FloorName, sep, adapter.RoomName, sep, user?.Workplace.Name);
    }

    private async Task<Rack> GetRackAsync(AdapterDetails adapter, CancellationToken cancellationToken)
    {
        if (adapter.NetworkDeviceID != 0)
        {
            var networkDevice = await _networkDeviceRepository.With(x => x.Rack)
                .FirstOrDefaultAsync(x => x.ID == adapter.NetworkDeviceID, cancellationToken);
            return networkDevice?.Rack;
        }
        return null;
    }

    public Task<IEnumerable<AdapterDetails>> BuildManyAsync(IEnumerable<Adapter> dataItems, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}