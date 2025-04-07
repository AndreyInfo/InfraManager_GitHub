using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Enumerable = System.Linq.Enumerable;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests;

public class ChangeRequestTypeQueryBuilder :
    IBuildEntityQuery<ChangeRequestType, RfcTypeDetailsModel, LookupListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<ChangeRequestType, RfcTypeDetailsModel, LookupListFilter>>
{
    
    private readonly IReadonlyRepository<ChangeRequestType> _repository;

    public ChangeRequestTypeQueryBuilder(IReadonlyRepository<ChangeRequestType> repository)
    {
        _repository = repository;
    }
    
    public IExecutableQuery<ChangeRequestType> Query(LookupListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrEmpty(filterBy.SearchName))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterBy.SearchName.ToLower()));
        }

        return query;
    }
}