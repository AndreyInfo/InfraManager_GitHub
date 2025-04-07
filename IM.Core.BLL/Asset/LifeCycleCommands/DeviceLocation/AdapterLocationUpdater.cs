using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Location;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.LifeCycleCommands.DeviceLocation;
internal class AdapterLocationUpdater : IDeviceLocationUpdater
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Adapter> _repository;
    private readonly IReadonlyRepository<Room> _roomRepository;
    private readonly IReadonlyRepository<NetworkDevice> _networkDeviceRepository;

    public AdapterLocationUpdater(IRepository<Adapter> repository
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Room> roomRepository
        , IReadonlyRepository<NetworkDevice> networkDeviceRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _roomRepository = roomRepository;
        _networkDeviceRepository = networkDeviceRepository;
    }

    public async Task<DeviceLocationDetails> UpdateDeviceLocationAsync(Guid id, DeviceLocationData data, CancellationToken cancellationToken)
    {
        var details = new DeviceLocationDetails();

        var adapter = await _repository
            .FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Adapter);

        if (data.RoomID is not null)
        {
            var room = await _roomRepository.FirstOrDefaultAsync(x => x.IMObjID == data.RoomID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(data.RoomID.Value, ObjectClass.Room);

            adapter.RoomID = room.ID;
            adapter.TerminalDeviceID = null;
            adapter.NetworkDeviceID = null;

            details.LocationID = data.RoomID.Value;
            details.LocationClassID = ObjectClass.Room;
        }
        else if (data.NetworkDeviceID is not null)
        {
            var networkDevice = await _networkDeviceRepository.FirstOrDefaultAsync(x => x.IMObjID == data.NetworkDeviceID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(data.NetworkDeviceID.Value, ObjectClass.ActiveDevice);

            adapter.NetworkDeviceID = networkDevice.ID;
            adapter.RoomID = networkDevice.RoomID;
            adapter.TerminalDeviceID = null;

            details.LocationID = networkDevice.IMObjID;
            details.LocationClassID = ObjectClass.ActiveDevice;
        }

        await _unitOfWork.SaveAsync();

        return details;
    }
}
