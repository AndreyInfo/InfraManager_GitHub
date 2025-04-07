using Inframanager.BLL;
using InfraManager.DAL.Software;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL;

namespace InfraManager.BLL.Software.SoftwareModelUsingTypes;

internal class SoftwareModelUsingTypeQueryBuilder :
    IBuildEntityQuery<SoftwareModelUsingType, SoftwareModelUsingTypeDetails, LookupListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<SoftwareModelUsingType, SoftwareModelUsingTypeDetails, LookupListFilter>>
{
    private readonly IReadonlyRepository<SoftwareModelUsingType> _repository;

    public SoftwareModelUsingTypeQueryBuilder(IReadonlyRepository<SoftwareModelUsingType> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<SoftwareModelUsingType> Query(LookupListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.SearchName))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterBy.SearchName.ToLower()));
        }

        return query;
    }
}
