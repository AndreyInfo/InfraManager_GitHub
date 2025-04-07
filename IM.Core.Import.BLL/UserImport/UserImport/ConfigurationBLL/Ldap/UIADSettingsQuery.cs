using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADSettingsQuery : IFilterEntity<UIADSetting, UIADSettingsFilter>,
    ISelfRegisteredService<IFilterEntity<UIADSetting, UIADSettingsFilter>>
{
    private readonly IReadonlyRepository<UIADSetting> _repository;

    public UIADSettingsQuery(IReadonlyRepository<UIADSetting> repository)
    {
        _repository = repository;
    }

    public IQueryable<UIADSetting> Query(UIADSettingsFilter filter)
    {
        var query = _repository
            .Query();

        if (filter.ADConfigurationID.HasValue)
            query = query.Where(x => x.ADConfigurationID == filter.ADConfigurationID);


        return query;
    }
}