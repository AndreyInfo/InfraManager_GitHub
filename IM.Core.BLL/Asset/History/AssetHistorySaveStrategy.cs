using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Asset.History;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.Asset.History;

/// <summary>
/// Стратегия сохранения истории имущественных операций.
/// </summary>
public abstract class AssetHistorySaveStrategy
{
    protected readonly IMapper Mapper;
    protected readonly IUnitOfWork UnitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IReadonlyRepository<User> _userRepository;
    private readonly IReadonlyRepository<LifeCycleState> _lifeCycleStateRepository;
    private readonly IRepository<AssetHistory> _assetHistoryRepository;
    private readonly IRepository<AssetHistoryObject> _assetHistoryObjectRepository;
    private readonly IRepository<AssetHistoryChangeAssetState> _assetHistoryChangeAssetStateRepository;
    private readonly IServiceMapper<ObjectClass, IObjectHistoryNameGetter> _objectNameFinder;

    public AssetHistorySaveStrategy(IMapper mapper
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IReadonlyRepository<User> userRepository
        , IReadonlyRepository<LifeCycleState> lifeCycleStateRepository
        , IRepository<AssetHistory> assetHistoryRepository
        , IRepository<AssetHistoryObject> assetHistoryObjectRepository
        , IRepository<AssetHistoryChangeAssetState> assetHistoryChangeAssetStateRepository
        , IServiceMapper<ObjectClass, IObjectHistoryNameGetter> objectNameFinder)
    {
        Mapper = mapper;
        UnitOfWork = unitOfWork;
        _currentUser = currentUser;
        _userRepository = userRepository;
        _objectNameFinder = objectNameFinder;
        _assetHistoryRepository = assetHistoryRepository;
        _lifeCycleStateRepository = lifeCycleStateRepository;
        _assetHistoryObjectRepository = assetHistoryObjectRepository;
        _assetHistoryChangeAssetStateRepository = assetHistoryChangeAssetStateRepository;
    }

    /// <summary>
    /// Сохранение истории выполнения конкретной команды для объекта.
    /// </summary>
    /// <param name="historyID">Идентификатор истории.</param>
    /// <param name="data">Данные для истории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    protected abstract Task SaveCommandHistoryAsync(Guid historyID, AssetHistoryBaseData data, CancellationToken cancellationToken);

    /// <summary>
    /// Сохранение всей истории имущественной операции.
    /// </summary>
    /// <param name="objectID">Идентификатор объекта.</param>
    /// <param name="data">Данные для истории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task SaveAsync(Guid objectID, AssetHistoryBaseData data, CancellationToken cancellationToken)
    {
        using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
        {
            var assetHistoryID = await SaveAssetHistoryAsync(data, cancellationToken);

            await SaveAssetHistoryObjectAsync(assetHistoryID, objectID, data, cancellationToken);
            await SaveCommandHistoryAsync(assetHistoryID, data, cancellationToken);
            await SaveAssetHistoryChangeAssetStateAsync(assetHistoryID, data, cancellationToken);

            await UnitOfWork.SaveAsync(cancellationToken);
            transaction.Complete();
        }
    }

    /// <summary>
    /// Сохранение общей истории выполнения команды.
    /// </summary>
    /// <param name="data">Данные для истории.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Идентификатор истории.</returns>
    private async Task<Guid> SaveAssetHistoryAsync(AssetHistoryBaseData data, CancellationToken cancellationToken)
    {
        var assetHistory = new AssetHistory();

        var user = await _userRepository.FirstOrDefaultAsync(x => x.IMObjID == _currentUser.UserId, cancellationToken);
        Mapper.Map(data, assetHistory);
        Mapper.Map(user, assetHistory);

        _assetHistoryRepository.Insert(assetHistory);

        await UnitOfWork.SaveAsync(cancellationToken);

        return assetHistory.ID;
    }

    /// <summary>
    /// Сохранение связи истории и объекта для которого выполнялась команда.
    /// </summary>
    /// <param name="objectID">Идентификатор объекта.</param>
    /// <param name="historyID">Идентификатор истории.</param>
    /// <param name="data">Данные для истории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    private async Task SaveAssetHistoryObjectAsync(Guid historyID, Guid objectID, AssetHistoryBaseData data, CancellationToken cancellationToken)
    {
        var objectName = await _objectNameFinder.Map(data.ClassID).GetAsync(objectID, cancellationToken);
        var assetHistoryObject = new AssetHistoryObject(historyID, objectID, data.ClassID, objectName);

        _assetHistoryObjectRepository.Insert(assetHistoryObject);

        await UnitOfWork.SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Сохранение связи истории и состояния жизненного цикла объекта для которого выполнялась команда.
    /// </summary>
    /// <param name="historyID">Идентификатор истории.</param>
    /// <param name="data">Данные для истории.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    private async Task SaveAssetHistoryChangeAssetStateAsync(Guid historyID, AssetHistoryBaseData data, CancellationToken cancellationToken)
    {
        var lifeCycleState = await _lifeCycleStateRepository.FirstOrDefaultAsync(x => x.ID == data.LifeCycleStateID, cancellationToken);
        var assetHistoryChangeAssetState = new AssetHistoryChangeAssetState(historyID, data.LifeCycleStateID, lifeCycleState?.Name ?? "");

        _assetHistoryChangeAssetStateRepository.Insert(assetHistoryChangeAssetState);

        await UnitOfWork.SaveAsync(cancellationToken);
    }
}
