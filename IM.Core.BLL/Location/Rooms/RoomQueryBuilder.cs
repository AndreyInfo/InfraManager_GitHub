using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Rooms;

public class RoomQueryBuilder :
    IBuildEntityQuery<Room, RoomDetails, RoomListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<Room, RoomDetails, RoomListFilter>>
{
    private readonly IReadonlyRepository<Room> _repository;

    public RoomQueryBuilder(IReadonlyRepository<Room> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<Room> Query(RoomListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.Name))
        {
            query = query.Where(room => room.Name.ToLower().Contains(filterBy.Name.ToLower()));
        }

        if (filterBy.IMObjID.HasValue)
        {
            query = query.Where(room => room.IMObjID == filterBy.IMObjID);
        }

        if (filterBy.FloorID.HasValue)
        {
            query = query.Where(room => room.FloorID == filterBy.FloorID);
        }

        return query;
    }
}