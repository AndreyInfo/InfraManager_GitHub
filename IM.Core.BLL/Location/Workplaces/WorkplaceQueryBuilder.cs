using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Workplaces;

public class WorkplaceQueryBuilder :
    IBuildEntityQuery<Workplace, WorkplaceDetails, WorkplaceListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<Workplace, WorkplaceDetails, WorkplaceListFilter>>
{
    private readonly IReadonlyRepository<Workplace> _repository;

    public WorkplaceQueryBuilder(IReadonlyRepository<Workplace> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<Workplace> Query(WorkplaceListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.Name))
        {
            query = query.Where(workplace => workplace.Name.ToLower().Contains(filterBy.Name.ToLower()));
        }

        if (filterBy.IMObjID.HasValue)
        {
            query = query.Where(workplace => workplace.IMObjID == filterBy.IMObjID);
        }

        if (filterBy.RoomID.HasValue)
        {
            query = query.Where(workplace => workplace.RoomID == filterBy.RoomID);
        }

        if (filterBy.RoomIMObjID.HasValue)
        {
            query = query.Where(workplace => workplace.Room.IMObjID == filterBy.RoomIMObjID);
        }

        return query;
    }
}