using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software.SoftwareLicenses;

internal class SoftwareLicenseQueryBuilder :
    IBuildEntityQuery<SoftwareLicence, SoftwareLicenseDetails, LookupListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<SoftwareLicence, SoftwareLicenseDetails, LookupListFilter>>
{
    private readonly IReadonlyRepository<SoftwareLicence> _repository;

    public SoftwareLicenseQueryBuilder(IReadonlyRepository<SoftwareLicence> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<SoftwareLicence> Query(LookupListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.SearchName))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterBy.SearchName.ToLower()));
        }

        return query;
    }
}
