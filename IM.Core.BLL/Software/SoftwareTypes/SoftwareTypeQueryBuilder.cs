using Inframanager.BLL;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL;
using SoftwareTypeEntity = InfraManager.DAL.Software.SoftwareType;

namespace InfraManager.BLL.Software.SoftwareTypes;

internal class SoftwareTypeQueryBuilder :
    IBuildEntityQuery<SoftwareTypeEntity, SoftwareTypeDetails, LookupListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<SoftwareTypeEntity, SoftwareTypeDetails, LookupListFilter>>
{
    private readonly IReadonlyRepository<SoftwareTypeEntity> _repository;

    public SoftwareTypeQueryBuilder(IReadonlyRepository<SoftwareTypeEntity> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<SoftwareTypeEntity> Query(LookupListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.SearchName))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterBy.SearchName.ToLower()));
        }

        return query;
    }
}
