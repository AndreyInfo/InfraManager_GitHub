using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Configuration;

namespace InfraManager.BLL.Technologies;
internal sealed class TechnologyTypeQueryBuilder :
     IBuildEntityQuery<TechnologyType, TechnologyTypeDetails, TechnologyTypeFilter>
     , ISelfRegisteredService<IBuildEntityQuery<TechnologyType, TechnologyTypeDetails, TechnologyTypeFilter>>
{
    private readonly IReadonlyRepository<TechnologyType> _technologyTypes;

    public TechnologyTypeQueryBuilder(IReadonlyRepository<TechnologyType> technologyTypes)
    {
        _technologyTypes = technologyTypes;
    }

    public IExecutableQuery<TechnologyType> Query(TechnologyTypeFilter filterBy)
    {
        var query = _technologyTypes.Query();

        if(!string.IsNullOrEmpty(filterBy.SearchString))
            query = query.Where(c=> c.Name.ToLower().Contains(filterBy.SearchString.ToLower()));

        return query;
    }
}
