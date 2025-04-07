using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADConfigurationsQuery :
    IFilterEntity<UIADConfiguration, UIADConfigurationsFilter>,
    ISelfRegisteredService<
        IFilterEntity<UIADConfiguration, UIADConfigurationsFilter>>
{
    private readonly IReadonlyRepository<UIADConfiguration> _repository;

    public UIADConfigurationsQuery(IReadonlyRepository<UIADConfiguration> repository)
    {
        _repository = repository;
    }

    public IQueryable<UIADConfiguration> Query(UIADConfigurationsFilter filter)
    {
        var query = _repository
            .Query();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.ShowUsersInADTree.HasValue)
            query = query.Where(x => x.ShowUsersInADTree == filter.ShowUsersInADTree.Value);

        if (!string.IsNullOrEmpty(filter.Note))
            query = query.Where(x => x.Note.Contains(filter.Note));


        return query;
    }
}