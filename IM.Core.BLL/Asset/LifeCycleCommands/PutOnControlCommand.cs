using AutoMapper;
using InfraManager.BLL.Asset.History;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
internal sealed class PutOnControlCommand : LifeCycleCommandWithAlert
{
    private readonly ILifeCycleCommandWithAlertExecutor _executor;
    private readonly ILifeCycleStateTransitionChecker _transitionChecker;

    public PutOnControlCommand(IMapper mapper
        , IUnitOfWork unitOfWork
        , IRepository<AssetEntity> assetRepository
        , ILifeCycleCommandWithAlertExecutor executor
        , ILifeCycleStateTransitionChecker transitionChecker
        , IServiceMapper<LifeCycleOperationCommandType, AssetHistorySaveStrategy> assetHistorySaver)
        : base(mapper,unitOfWork, assetRepository, assetHistorySaver)
    {
        _executor = executor;
        _transitionChecker = transitionChecker;
    }

    public async override Task<LifeCycleCommandResultItem> ExecuteWithAlertAsync(Guid id, Guid operationID, LifeCycleCommandBaseData data, CancellationToken cancellationToken)
    {
        var commandResult = await _executor.ExecuteAsync(id, data, cancellationToken);

        var stateID = await _transitionChecker.GetStateAsync(operationID, Mapper.Map<LifeCycleCommandResultItem>(commandResult), cancellationToken);

        await ChangeLifeCycleStateAsync(id, stateID, cancellationToken);

        return commandResult;
    }
}
