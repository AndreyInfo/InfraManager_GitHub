using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADIMClassConcordancesQuery : IFilterEntity<UIADIMClassConcordance,
        UIADIMClassConcordancesFilter>,
    ISelfRegisteredService<IFilterEntity<UIADIMClassConcordance,
        UIADIMClassConcordancesFilter>>
{
    private readonly IReadonlyRepository<UIADIMClassConcordance> _repository;

    public UIADIMClassConcordancesQuery(IReadonlyRepository<UIADIMClassConcordance> repository)
    {
        _repository = repository;
    }

    public IQueryable<UIADIMClassConcordance> Query(UIADIMClassConcordancesFilter filter)
    {
        var query = _repository
            .Query();


        return query;
    }
}