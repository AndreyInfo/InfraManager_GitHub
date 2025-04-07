using AutoMapper;
using InfraManager.BLL.Asset.LifeCycleCommands.DeviceLocation;
using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
internal sealed class MoveCommandExecutor : ILifeCycleCommandExecutor
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<AssetEntity> _assetRepository;
    private readonly IServiceMapper<ObjectClass, IDeviceLocationUpdater> _deviceLocationUpdater;

    public MoveCommandExecutor(IMapper mapper
        , IUnitOfWork unitOfWork
        , IRepository<AssetEntity> assetRepository
        , IServiceMapper<ObjectClass, IDeviceLocationUpdater> deviceLocationUpdater)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _assetRepository = assetRepository;
        _deviceLocationUpdater = deviceLocationUpdater;
    }

    public async Task<LifeCycleCommandResultItem> ExecuteAsync(Guid id, LifeCycleCommandBaseData data, CancellationToken cancellationToken)
    {
        using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
        {
            var newLocation = await _deviceLocationUpdater.Map(data.ClassID)
                .UpdateDeviceLocationAsync(id, _mapper.Map<DeviceLocationData>(data), cancellationToken);

            await SetAssetDataAsync(id, data, cancellationToken);

            transaction.Complete();

            return _mapper.Map<LifeCycleCommandResultItem>(newLocation);
        }
    }

    private async Task SetAssetDataAsync(Guid id, LifeCycleCommandBaseData data, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
        var assetData = _mapper.Map<AssetData>(asset);
        _mapper.Map(data, assetData);

        if (data.NetworkDeviceID is not null)
        {
            var networkDevice = await _assetRepository
                .FirstOrDefaultAsync(x => x.ID == data.NetworkDeviceID, cancellationToken);

            assetData.UtilizerID = networkDevice.UtilizerID;
            assetData.UtilizerClassID = networkDevice.UtilizerClassID;
        }

        _mapper.Map(assetData, asset);

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
