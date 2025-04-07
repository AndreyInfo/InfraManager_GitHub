using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Asset.History;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.History;
internal sealed class ToRepairSaveStrategy : AssetHistorySaveStrategy
{
    private readonly IReadonlyRepository<ServiceCenter> _serviceCenterRepository;
    private readonly IReadonlyRepository<ServiceContract> _serviceContractRepository;
    private readonly IRepository<AssetHistoryToRepair> _assetHistoryToRepairRepository;
    public ToRepairSaveStrategy(IMapper mapper
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IReadonlyRepository<User> userRepository
        , IReadonlyRepository<LifeCycleState> lifeCycleStateRepository
        , IRepository<AssetHistory> assetHistoryRepository
        , IReadonlyRepository<ServiceCenter> serviceCenterRepository
        , IReadonlyRepository<ServiceContract> serviceContractRepository
        , IRepository<AssetHistoryObject> assetHistoryObjectRepository
        , IRepository<AssetHistoryToRepair> assetHistoryToRepairRepository
        , IRepository<AssetHistoryChangeAssetState> assetHistoryChangeAssetStateRepository
        , IServiceMapper<ObjectClass, IObjectHistoryNameGetter> objectNameFinder) 
        : base(mapper
            , unitOfWork
            , currentUser
            , userRepository
            , lifeCycleStateRepository
            , assetHistoryRepository
            , assetHistoryObjectRepository
            , assetHistoryChangeAssetStateRepository
            , objectNameFinder)
    {
        _serviceCenterRepository = serviceCenterRepository;
        _serviceContractRepository = serviceContractRepository;
        _assetHistoryToRepairRepository = assetHistoryToRepairRepository;
    }

    protected override async Task SaveCommandHistoryAsync(Guid historyID, AssetHistoryBaseData data, CancellationToken cancellationToken)
    {
        var assetHistoryToRepair = Mapper.Map<AssetHistoryToRepair>(data);

        if (data.ServiceCenterID is not null)
        {
            var serviceCenter = await _serviceCenterRepository.FirstOrDefaultAsync(x => x.ID == data.ServiceCenterID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(data.ServiceCenterID.Value, ObjectClass.ServiceCenter);

            assetHistoryToRepair.ServiceCenterName = serviceCenter.Name;
        }

        if (data.ServiceContractID is not null)
        {
            var serviceContractNumber = await _serviceContractRepository
                .FirstOrDefaultAsync(x => x.ID == data.ServiceContractID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(data.ServiceContractID.Value, ObjectClass.ServiceContract);

            assetHistoryToRepair.ServiceContractNumber = serviceContractNumber.Number.ToString();
        }

        _assetHistoryToRepairRepository.Insert(assetHistoryToRepair);
        await UnitOfWork.SaveAsync(cancellationToken);
    }
}
