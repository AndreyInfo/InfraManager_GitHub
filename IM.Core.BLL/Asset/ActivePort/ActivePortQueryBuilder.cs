using Inframanager.BLL;
using InfraManager.DAL;
using ActivePortEntity = InfraManager.DAL.Asset.ActivePort;

namespace InfraManager.BLL.Asset.ActivePort;

internal sealed class ActivePortQueryBuilder : 
    IBuildEntityQuery<ActivePortEntity, ActivePortDetails, ActivePortFilter>
    , ISelfRegisteredService<IBuildEntityQuery<ActivePortEntity, ActivePortDetails, ActivePortFilter>>
{
    private readonly IReadonlyRepository<ActivePortEntity> _repository;

    public ActivePortQueryBuilder(IReadonlyRepository<ActivePortEntity> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<ActivePortEntity> Query(ActivePortFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.PortName))
        {
            query = query.Where(x => x.PortName.ToLower().Contains(filterBy.PortName.ToLower()));
        }

        if (filterBy.ActiveEquipmentID.HasValue)
        {
            query = query.Where(x => x.ActiveEquipmentID == filterBy.ActiveEquipmentID);
        }

        return query;
    }
}
