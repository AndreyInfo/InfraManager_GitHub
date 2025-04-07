using Inframanager.BLL;
using InfraManager.BLL.Asset.Filters;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Location.Racks;

public class RackQueryBuilder : IBuildEntityQuery<Rack, RackDetails, RackListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<Rack, RackDetails, RackListFilter>>
{
    private readonly IReadonlyRepository<Rack> _repository;

    public RackQueryBuilder(IReadonlyRepository<Rack> repository) => _repository = repository;

    public IExecutableQuery<Rack> Query(RackListFilter filterBy)
    {
        var query = _repository
            .With(rack => rack.Room)
            .ThenWith(rack => rack.Floor)
            .ThenWith(rack => rack.Building)
            .ThenWith(rack => rack.Organization)
            .Query();

        if (filterBy.IMObjID is not null)
            query = query.Where(rack => rack.IMObjID == filterBy.IMObjID);

        if (filterBy.RoomIMObjID is not null)
            query = query.Where(rack => rack.Room.IMObjID == filterBy.RoomIMObjID);

        return query;
    }
}