using Inframanager.BLL;
using InfraManager.DAL;
using PortAdapterEntity = InfraManager.DAL.Asset.PortAdapter;

namespace InfraManager.BLL.Asset.PortAdapter;

internal class PortAdapterBuildQuery : 
    IBuildEntityQuery<PortAdapterEntity, PortAdapterDetails, PortAdapterFilter>
    , ISelfRegisteredService<IBuildEntityQuery<PortAdapterEntity, PortAdapterDetails, PortAdapterFilter>>
{
    private readonly IReadonlyRepository<PortAdapterEntity> _repository;

    public PortAdapterBuildQuery(IReadonlyRepository<PortAdapterEntity> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<PortAdapterEntity> Query(PortAdapterFilter filterBy)
    {
        var query = _repository.With(x => x.JackType).With(x => x.TechnologyType)
            .Query(x => x.ObjectID == filterBy.ObjectID);

        if (!string.IsNullOrEmpty(filterBy.SearchString))
            query.Where(x => x.PortNumber.ToString().Contains(filterBy.SearchString));

        return query;
    }
}