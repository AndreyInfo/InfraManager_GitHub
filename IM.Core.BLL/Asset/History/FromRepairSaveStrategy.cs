using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Asset.History;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.History;
internal sealed class FromRepairSaveStrategy : AssetHistorySaveStrategy
{
    private readonly IRepository<AssetHistoryFromRepair> _repository;

    public FromRepairSaveStrategy(IMapper mapper
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IReadonlyRepository<User> userRepository
        , IReadonlyRepository<LifeCycleState> lifeCycleStateRepository
        , IRepository<AssetHistoryFromRepair> repository
        , IRepository<AssetHistory> assetHistoryRepository
        , IRepository<AssetHistoryObject> assetHistoryObjectRepository
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
        _repository = repository;
    }

    protected override async Task SaveCommandHistoryAsync(Guid historyID, AssetHistoryBaseData data, CancellationToken cancellationToken)
    {
        var assetHistoryFromRepair = Mapper.Map<AssetHistoryFromRepair>(data);

        _repository.Insert(assetHistoryFromRepair);

        await UnitOfWork.SaveAsync(cancellationToken);
    }
}
