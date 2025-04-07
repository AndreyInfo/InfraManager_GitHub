using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import;

public class UIDBSettingsQueryBuilder : IFilterEntity<UIDBSettings, UIDBSettingsFilter>,
    ISelfRegisteredService<IFilterEntity<UIDBSettings, UIDBSettingsFilter>>
{
    private readonly IReadonlyRepository<UIDBSettings> _repository;

    public UIDBSettingsQueryBuilder(IReadonlyRepository<UIDBSettings> repository)
    {
        _repository = repository;
    }

    public IQueryable<UIDBSettings> Query(UIDBSettingsFilter filter)
    {
        var query = _repository
            .Query();

        if (filter.DBConfigurationID.HasValue)
            query = query.Where(x => x.DBConfigurationID == filter.DBConfigurationID);

        if (!string.IsNullOrWhiteSpace(filter.DatabaseName))
            query = query.Where(x => x.DatabaseName == filter.DatabaseName);
        return query;
    }
}