using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADIMFieldConcordancesQuery : IFilterEntity<UIADIMFieldConcordance,
        UIADIMFieldConcordancesFilter>,
    ISelfRegisteredService<IFilterEntity<UIADIMFieldConcordance,
        UIADIMFieldConcordancesFilter>>
{
    private readonly IReadonlyRepository<UIADIMFieldConcordance> _repository;

    public UIADIMFieldConcordancesQuery(IReadonlyRepository<UIADIMFieldConcordance> repository)
    {
        _repository = repository;
    }

    public IQueryable<UIADIMFieldConcordance> Query(UIADIMFieldConcordancesFilter filter)
    {
        var query = _repository
            .Query();

        if (!string.IsNullOrEmpty(filter.Expression))
            query = query.Where(x => x.Expression.Contains(filter.Expression));

        if (filter.ConfigurationID.HasValue)
            query = query.Where(x => x.ConfigurationID == filter.ConfigurationID.Value);
        
        return query;
    }
}