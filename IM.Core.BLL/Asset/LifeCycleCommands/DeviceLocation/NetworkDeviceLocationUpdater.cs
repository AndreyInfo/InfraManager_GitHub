using InfraManager.DAL.Asset;
using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Asset.LifeCycleCommands.DeviceLocation;
internal sealed class NetworkDeviceLocationUpdater : IDeviceLocationUpdater
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<NetworkDevice> _repository;
    private readonly IReadonlyRepository<Room> _roomRepository;
    private readonly IReadonlyRepository<Rack> _rackRepository;

    public NetworkDeviceLocationUpdater(IRepository<NetworkDevice> repository
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Room> roomRepository
        , IReadonlyRepository<Rack> rackRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _roomRepository = roomRepository;
        _rackRepository = rackRepository;
    }

    public async Task<DeviceLocationDetails> UpdateDeviceLocationAsync(Guid id, DeviceLocationData data, CancellationToken cancellationToken)
    {
        var details = new DeviceLocationDetails();

        var networkDevice = await _repository.FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.ActiveDevice);

        if (data.RoomID is not null)
        {
            var room = await _roomRepository.FirstOrDefaultAsync(x => x.IMObjID == data.RoomID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(data.RoomID.Value, ObjectClass.Room);

            networkDevice.RoomID = room.ID;

            details.LocationID = data.RoomID.Value;
            details.LocationClassID = ObjectClass.Room;
        }
        else if (data.RackID is not null)
        {
            var rack = await _rackRepository.FirstOrDefaultAsync(x => x.IMObjID == data.RackID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(data.RackID.Value, ObjectClass.Rack);

            networkDevice.RackID = rack.ID;

            details.LocationID = data.RackID.Value;
            details.LocationClassID = ObjectClass.Rack;
        }

        await _unitOfWork.SaveAsync();

        return details;
    }
}
