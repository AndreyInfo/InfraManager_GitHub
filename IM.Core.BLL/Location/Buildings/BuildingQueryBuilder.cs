using InfraManager.DAL.Location;
using Inframanager.BLL;
using InfraManager.DAL;

namespace InfraManager.BLL.Location.Buildings;

internal class BuildingQueryBuilder :
    IBuildEntityQuery<Building, BuildingDetails, BuildingListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<Building, BuildingDetails, BuildingListFilter>>
{
    private readonly IReadonlyRepository<Building> _repository;

    public BuildingQueryBuilder(IReadonlyRepository<Building> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<Building> Query(BuildingListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.Name))
        {
            query = query.Where(building => building.Name.ToLower().Contains(filterBy.Name.ToLower()));
        }

        if (filterBy.IMObjID.HasValue)
        {
            query = query.Where(building => building.IMObjID == filterBy.IMObjID);
        }

        if (filterBy.OrganizationID.HasValue)
        {
            query = query.Where(building => building.OrganizationID == filterBy.OrganizationID);
        }

        return query;
    }
}