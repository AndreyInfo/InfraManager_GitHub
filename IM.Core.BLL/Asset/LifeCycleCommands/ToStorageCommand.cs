using AutoMapper;
using InfraManager.BLL.Asset.History;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
internal sealed class ToStorageCommand : LifeCycleCommand
{
    private readonly ILifeCycleStateTransitionChecker _checker;
    private readonly IServiceMapper<LifeCycleOperationCommandType, ILifeCycleCommandExecutor> _executor;

    public ToStorageCommand(IMapper mapper
        , IUnitOfWork unitOfWork
        , IRepository<AssetEntity> assetRepository
        , ILifeCycleStateTransitionChecker checker
        , IServiceMapper<LifeCycleOperationCommandType, ILifeCycleCommandExecutor> executor
        , IServiceMapper<LifeCycleOperationCommandType, AssetHistorySaveStrategy> assetHistorySaver)
        : base(mapper, unitOfWork, assetRepository, assetHistorySaver)
    {
        _checker = checker;
        _executor = executor;
    }

    public override async Task<LifeCycleCommandResultItem> ExecuteAsync(Guid id, Guid operationID, LifeCycleCommandBaseData data, CancellationToken cancellationToken)
    {
        var commandResult = await _executor.Map(LifeCycleOperationCommandType.ToStorage).ExecuteAsync(id, data, cancellationToken);

        var stateID = await _checker.GetStateAsync(operationID, commandResult, cancellationToken);
        commandResult.LifeCycleStateID = stateID;

        await ChangeLifeCycleStateAsync(id, stateID, cancellationToken);

        return commandResult;
    }
}
