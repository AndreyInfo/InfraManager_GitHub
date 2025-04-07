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
internal sealed class RegistrationHistorySaveStrategy : AssetHistorySaveStrategy
{
    private readonly IOwnerNameQuery _ownerNameQuery;
    private readonly IRepository<AssetHistoryRegistration> _repository;
    private readonly IServiceMapper<ObjectClass, ILocationFullPathGetter> _locationFullPathFinder;

    public RegistrationHistorySaveStrategy(IMapper mapper
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IOwnerNameQuery ownerNameQuery
        , IReadonlyRepository<User> userRepository
        , IReadonlyRepository<LifeCycleState> lifeCycleStateRepository
        , IRepository<AssetHistoryRegistration> repository
        , IRepository<AssetHistory> assetHistoryRepository
        , IRepository<AssetHistoryObject> assetHistoryObjectRepository
        , IServiceMapper<ObjectClass, IObjectHistoryNameGetter> objectNameFinder
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
        _repository = repository;
        _ownerNameQuery = ownerNameQuery;
        _locationFullPathFinder = locationFullPathFinder;
    }

    protected override async Task SaveCommandHistoryAsync(Guid historyID, AssetHistoryBaseData data, CancellationToken cancellationToken)
    {
        var assetHistoryRegistration = Mapper.Map<AssetHistoryRegistration>(data);

        if (data.LocationID is not null)
            assetHistoryRegistration.NewLocationName = await _locationFullPathFinder.Map(data.LocationClassID.Value)
                .GetAsync(data.LocationID.Value, cancellationToken);

        if (data.OwnerID is not null)
            assetHistoryRegistration.OwnerName = await _ownerNameQuery.ExecuteAsync(data.OwnerID.Value, cancellationToken);

        _repository.Insert(assetHistoryRegistration);
        await UnitOfWork.SaveAsync(cancellationToken);
    }
}
