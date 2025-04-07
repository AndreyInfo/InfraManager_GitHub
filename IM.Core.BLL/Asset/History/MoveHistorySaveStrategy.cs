using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Asset.History;
using InfraManager.DAL.Location;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.History;
internal sealed class MoveHistorySaveStrategy : AssetHistorySaveStrategy
{
    private readonly IRepository<AssetHistoryMove> _assetHistoryMoveRepository;
    private readonly IServiceMapper<ObjectClass, ILocationFullPathGetter> _locationFullPathFinder;
    private readonly IUtilizerNameQuery _utilizerNameQuery;

    public MoveHistorySaveStrategy(IMapper mapper
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IUtilizerNameQuery utilizerNameQuery
        , IReadonlyRepository<User> userRepository
        , IReadonlyRepository<LifeCycleState> lifeCycleStateRepository
        , IRepository<AssetHistory> assetHistoryRepository
        , IRepository<AssetHistoryObject> assetHistoryObjectRepository
        , IServiceMapper<ObjectClass, IObjectHistoryNameGetter> objectNameFinder
        , IRepository<AssetHistoryMove> assetHistoryMoveRepository
        , IRepository<AssetHistoryChangeAssetState> assetHistoryChangeAssetStateRepository
        , IServiceMapper<ObjectClass, ILocationFullPathGetter> locationFullPathFinder)
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
        _utilizerNameQuery = utilizerNameQuery;
        _assetHistoryMoveRepository = assetHistoryMoveRepository;
        _locationFullPathFinder = locationFullPathFinder;
    }

    protected override async Task SaveCommandHistoryAsync(Guid historyID, AssetHistoryBaseData data, CancellationToken cancellationToken)
    {
        var assetHistoryMove = new AssetHistoryMove(historyID);
        Mapper.Map(data, assetHistoryMove);

        if (data.LocationID is not null)
            assetHistoryMove.NewLocationName = await _locationFullPathFinder.Map(data.LocationClassID.Value)
                .GetAsync(data.LocationID.Value, cancellationToken);

        if (data.UtilizerID is not null)
            assetHistoryMove.UtilizerName = await _utilizerNameQuery.ExecuteAsync(data.UtilizerID.Value, cancellationToken);

        _assetHistoryMoveRepository.Insert(assetHistoryMove);
        await UnitOfWork.SaveAsync(cancellationToken);
    }
}
