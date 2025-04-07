using AutoMapper;
using InfraManager.BLL.Asset.History;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
internal class ChangeLifeCycleStateCommand : LifeCycleCommand
{
    private readonly ILifeCycleStateTransitionChecker _transitionChecker;

    public ChangeLifeCycleStateCommand(IMapper mapper
        , IUnitOfWork unitOfWork
        , IRepository<AssetEntity> assetRepository
        , ILifeCycleStateTransitionChecker transitionChecker
        , IServiceMapper<LifeCycleOperationCommandType, AssetHistorySaveStrategy> assetHistorySaver)
        : base(mapper, unitOfWork, assetRepository, assetHistorySaver)
    {
        _transitionChecker = transitionChecker;
    }

    public override async Task<LifeCycleCommandResultItem> ExecuteAsync(Guid id, Guid operationID, LifeCycleCommandBaseData data, CancellationToken cancellationToken)
    {
        var stateID = await _transitionChecker.GetStateAsync(operationID, null, cancellationToken);
        await ChangeLifeCycleStateAsync(id, stateID, cancellationToken);

        return new LifeCycleCommandResultItem(stateID);
    }
}
