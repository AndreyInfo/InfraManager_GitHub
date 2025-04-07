using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADPathsQuery : IFilterEntity<UIADPath, UIADPathsFilter>,
    ISelfRegisteredService<IFilterEntity<UIADPath, UIADPathsFilter>>
{
    private readonly IReadonlyRepository<UIADPath> _repository;

    public UIADPathsQuery(IReadonlyRepository<UIADPath> repository)
    {
        _repository = repository;
    }

    public IQueryable<UIADPath> Query(UIADPathsFilter filter)
    {
        var query = _repository
            .Query();

        if (filter.ADSettingID.HasValue)
            query = query.Where(x => x.ADSettingID == filter.ADSettingID);

        if (!string.IsNullOrEmpty(filter.Path))
            query = query.Where(x => x.Path.Contains(filter.Path));


        return query;
    }
}