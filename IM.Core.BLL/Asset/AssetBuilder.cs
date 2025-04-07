using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Asset;
internal sealed class AssetBuilder : IBuildObject<AssetEntity, AssetData>
    , ISelfRegisteredService<IBuildObject<AssetEntity, AssetData>>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<LifeCycleState> _lifeCycleStateRepository;
    private readonly ILocalizeText _localizeText;

    public AssetBuilder(
          IMapper mapper
        , IReadonlyRepository<LifeCycleState> lifeCycleStateRepository
        , ILocalizeText localizeText)
    {
        _mapper = mapper;
        _lifeCycleStateRepository = lifeCycleStateRepository;
        _localizeText = localizeText;
    }

    public async Task<AssetEntity> BuildAsync(AssetData data, CancellationToken cancellationToken = default)
    {
        var asset = _mapper.Map<AssetEntity>(data);

        if (data.LifeCycleStateID is null)
        {
            var lifeCycleState = await _lifeCycleStateRepository.FirstOrDefaultAsync(x => x.IsDefault == true
                && x.LifeCycle.Type == LifeCycleType.ITActive && !x.LifeCycle.Removed, cancellationToken)
            ?? throw new InvalidObjectException(_localizeText.Localize(nameof(Resources.LifeCycleStateNotFound)));


            asset.LifeCycleStateID = lifeCycleState.ID;
            asset.LifeCycleState = lifeCycleState;
        } 
        else
        {
            var lifeCycleState = await _lifeCycleStateRepository.FirstOrDefaultAsync(x => x.ID == data.LifeCycleStateID, cancellationToken)
            ?? throw new InvalidObjectException(_localizeText.Localize(nameof(Resources.LifeCycleStateNotFound)));

            asset.LifeCycleStateID = data.LifeCycleStateID;
            asset.LifeCycleState = lifeCycleState;
        }

        return asset;
    }

    public async Task<IEnumerable<AssetEntity>> BuildManyAsync(IEnumerable<AssetData> dataItems, CancellationToken cancellationToken = default)
    {
        var assets = new List<AssetEntity>();

        foreach (var item in dataItems)
        {
            assets.Add(await BuildAsync(item, cancellationToken));
        }

        return assets.ToArray();
    }
}
