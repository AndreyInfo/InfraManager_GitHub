using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import;

public class UIDBConnectionStringQueryBuilder : IFilterEntity<UIDBConnectionString, UIDBConnectionStringFilter>,
    ISelfRegisteredService<IFilterEntity<UIDBConnectionString, UIDBConnectionStringFilter>>
{
    private readonly IReadonlyRepository<UIDBConnectionString> _repository;

    public UIDBConnectionStringQueryBuilder(IReadonlyRepository<UIDBConnectionString> repository)
    {
        _repository = repository;
    }

    public IQueryable<UIDBConnectionString> Query(UIDBConnectionStringFilter filter)
    {
        var query = _repository
            .Query();

        if (filter.SettingsID.HasValue)
            query = query.Where(x => x.SettingsID == filter.SettingsID);

        if (!string.IsNullOrEmpty(filter.ConnectionString))
            query = query.Where(x => x.ConnectionString.Contains(filter.ConnectionString));

        if (filter.ImportSourceType.HasValue)
            query = query.Where(x => x.ImportSourceType == filter.ImportSourceType);
        
        return query;
    }
}