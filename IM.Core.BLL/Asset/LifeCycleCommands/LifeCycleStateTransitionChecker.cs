using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Location;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
internal sealed class LifeCycleStateTransitionChecker : ILifeCycleStateTransitionChecker
    , ISelfRegisteredService<ILifeCycleStateTransitionChecker>
{
    private readonly IReadonlyRepository<Room> _roomRepository;
    private readonly IReadonlyRepository<LifeCycleStateOperation> _operationRepository;
    private readonly IReadonlyRepository<StorageLocationReference> _storageLocationReferenceRepository;
    private readonly IReadonlyRepository<NetworkDevice> _networkDeviceRepository;

    public LifeCycleStateTransitionChecker(IReadonlyRepository<Room> roomRepository
        , IReadonlyRepository<NetworkDevice> networkDeviceRepository
        , IReadonlyRepository<LifeCycleStateOperation> operationRepository
        , IReadonlyRepository<StorageLocationReference> storageLocationReferenceRepository)
    {
        _roomRepository = roomRepository;
        _operationRepository = operationRepository;
        _networkDeviceRepository = networkDeviceRepository;
        _storageLocationReferenceRepository = storageLocationReferenceRepository;
    }

    public async Task<Guid?> GetStateAsync(Guid operationID, LifeCycleCommandResultItem? item, CancellationToken cancellationToken)
    {
        var state = new Guid?();
        var operation = await _operationRepository.FirstOrDefaultAsync(x => x.ID == operationID, cancellationToken);

        if (item is not null)
            state = await GetStateFromTransitionAsync(state, operation.Transitions.ToArray(), item, cancellationToken);

        return state;
    }

    private async Task<Guid?> GetStateFromTransitionAsync(Guid? state, LifeCycleStateOperationTransition[] transitons, LifeCycleCommandResultItem item, CancellationToken cancellationToken)
    {
        foreach (var transition in transitons)
        {
            if (transition.Mode is LifeCycleTransitionMode.TargetIsDevice)
            {
                state = await GetStateByTargetIsDeviceModeAsync(transition, item, cancellationToken);
                if (state is not null)
                    return state;
            }

            else if (transition.Mode is LifeCycleTransitionMode.NewLocationIsStorage)
                state = await GetStateByNewLocationIsStorageModeAsync(transition, item, cancellationToken);

            else if (transition.Mode is LifeCycleTransitionMode.ElseOrDefault)
                state = GetStateByDefaultMode(transition);
        }
        return state;
    }

    private async Task<Guid?> GetStateByTargetIsDeviceModeAsync(LifeCycleStateOperationTransition transition
        , LifeCycleCommandResultItem item
        , CancellationToken cancellationToken)
    {
        if (item.LocationClassID is ObjectClass.ActiveDevice 
            && await _networkDeviceRepository.AnyAsync(x => x.IMObjID == item.LocationID, cancellationToken))
                return transition.FinishStateID;
        return null;
    }

    private async Task<Guid?> GetStateByNewLocationIsStorageModeAsync(LifeCycleStateOperationTransition transition
        , LifeCycleCommandResultItem item
        , CancellationToken cancellationToken)
    {
        if (item.LocationClassID is ObjectClass.Room && await IsRoomOnStorageAsync(item.LocationID, cancellationToken))
            return transition.FinishStateID;
        return null;
    }

    private Guid GetStateByDefaultMode(LifeCycleStateOperationTransition transition)
        => transition.FinishStateID;

    private async Task<bool> IsRoomOnStorageAsync(Guid? roomID, CancellationToken cancellationToken)
    {
        return await _roomRepository.AnyAsync(x => x.IMObjID == roomID, cancellationToken) 
            && await _storageLocationReferenceRepository.AnyAsync(x => x.ObjectID == roomID, cancellationToken);
    }
}
