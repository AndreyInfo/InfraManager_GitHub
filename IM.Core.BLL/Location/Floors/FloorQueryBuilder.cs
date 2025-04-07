using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Floors;

internal class FloorQueryBuilder :
    IBuildEntityQuery<Floor, FloorDetails, FloorListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<Floor, FloorDetails, FloorListFilter>>
{
    private readonly IReadonlyRepository<Floor> _repository;

    public FloorQueryBuilder(IReadonlyRepository<Floor> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<Floor> Query(FloorListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.Name))
        {
            query = query.Where(floor => floor.Name.ToLower().Contains(filterBy.Name.ToLower()));
        }

        if (filterBy.IMObjID.HasValue)
        {
            query = query.Where(floor => floor.IMObjID == filterBy.IMObjID);
        }

        if (filterBy.BuildingID.HasValue)
        {
            query = query.Where(floor => floor.BuildingID == filterBy.BuildingID);
        }

        return query;
    }
}