using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import;

public class UIDBConfigurationQueryBuilder : IFilterEntity<UIDBConfiguration, UIDBConfigurationFilter>,
    ISelfRegisteredService<IFilterEntity<UIDBConfiguration, UIDBConfigurationFilter>>
{
    private readonly IReadonlyRepository<UIDBConfiguration> _repository;

    public UIDBConfigurationQueryBuilder(IReadonlyRepository<UIDBConfiguration> repository)
    {
        _repository = repository;
    }

    public IQueryable<UIDBConfiguration> Query(UIDBConfigurationFilter filter)
    {
        var query = _repository
            .Query();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (!string.IsNullOrEmpty(filter.Note))
            query = query.Where(x => x.Note.Contains(filter.Note));


        return query;
    }
}