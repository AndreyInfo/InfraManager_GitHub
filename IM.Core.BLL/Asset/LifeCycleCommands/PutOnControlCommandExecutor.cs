using AutoMapper;
using InfraManager.BLL.Asset.LifeCycleCommands.DeviceLocation;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
internal sealed class PutOnControlCommandExecutor : ILifeCycleCommandWithAlertExecutor, ISelfRegisteredService<ILifeCycleCommandWithAlertExecutor>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Adapter> _adapterRepository;
    private readonly IRepository<NetworkDevice> _networkDeviceRepository;
    private readonly IRepository<AssetEntity> _assetRepository;
    private readonly IServiceMapper<ObjectClass, IDeviceLocationUpdater> _deviceLocationUpdater;
    private readonly IServiceMapper<LifeCycleCommandAlertType, IAssignValueForComponentsStrategy> _assignValueStrategy;

    public PutOnControlCommandExecutor(IMapper mapper
        , IUnitOfWork unitOfWork
        , IRepository<AssetEntity> assetRepository
        , IRepository<Adapter> adapterRepository
        , IRepository<NetworkDevice> networkDeviceRepository
        , IServiceMapper<ObjectClass, IDeviceLocationUpdater> deviceLocationSetter
        , IServiceMapper<LifeCycleCommandAlertType, IAssignValueForComponentsStrategy> assignValueStrategy)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _assetRepository = assetRepository;
        _adapterRepository = adapterRepository;
        _networkDeviceRepository = networkDeviceRepository;
        _deviceLocationUpdater = deviceLocationSetter;
        _assignValueStrategy = assignValueStrategy;
    }

    public async Task<LifeCycleCommandResultItem> ExecuteAsync(Guid id, LifeCycleCommandBaseData data, CancellationToken cancellationToken)
    {
        using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
        {
            var newLocation = await _deviceLocationUpdater.Map(data.ClassID)
                .UpdateDeviceLocationAsync(id, _mapper.Map<DeviceLocationData>(data), cancellationToken);

            await SetAssetDataAsync(id, data, cancellationToken);

            // TODO: Генерация инв.номеров и кодов будет реализовано в рамках другой задачи.
            // await AssignInventoryNumberAsync(id, data, cancellationToken);
            // await AssignCode(id, data, cancellationToken);

            transaction.Complete();

            return _mapper.Map<LifeCycleCommandResultItem>(newLocation);
        }
    }

    private async Task AssignInventoryNumberAsync(Guid id, LifeCycleCommandBaseData data, LifeCycleCommandAlertData alertData, CancellationToken cancellationToken)
    {
        if (data.ClassID is ObjectClass.ActiveDevice)
        {
            var networkDevice = await _networkDeviceRepository.FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken);

            networkDevice.InvNumber = GenerateNetworkDeviceInventoryNumber();

            if (data.AssignInventoryNumberToComponents.Value)
            {
                /// TODO: Компонентам (адаптерам и периферийным устройствам), которые есть у сетевого оборудования
                /// присваивать инвентарный номер, предварительно спрашивая пользователя
                await _assignValueStrategy.Map(LifeCycleCommandAlertType.InventoryNumber)
                    .AssignValueForComponentsByDeviceIDAsync(id, alertData);
            }
        }

        if (data.ClassID is ObjectClass.Adapter)
        {
            var adapter = await _adapterRepository.FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken);

            adapter.Name = GenerateAdapterInventoryNumber();
        }
    }

    private string GenerateNetworkDeviceInventoryNumber()
    {
        var devices = _networkDeviceRepository.Query().Where(x => !string.IsNullOrEmpty(x.InvNumber)).ToArray();
        var maxInvNumber = devices.Max(x => int.Parse(x.InvNumber));
        return (maxInvNumber + 1).ToString();
    }

    private string GenerateAdapterInventoryNumber()
    {
        var devices = _adapterRepository.Query().Where(x => !string.IsNullOrEmpty(x.Name)).ToArray();
        var maxInvNumber = devices.Where(x => int.TryParse(x.Name, out _)).Max(x => int.Parse(x.Name));
        return (maxInvNumber + 1).ToString();
    }

    private async Task SetAssetDataAsync(Guid id, LifeCycleCommandBaseData data, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
        var assetData = _mapper.Map<AssetData>(asset);
        _mapper.Map(data, assetData);

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
