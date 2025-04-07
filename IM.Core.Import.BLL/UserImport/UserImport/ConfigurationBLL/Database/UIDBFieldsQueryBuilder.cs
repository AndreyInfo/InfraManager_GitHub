using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;
using InfraManager.DAL.Database.Import;
using InfraManager.ServiceBase.ImportService.DBService;

namespace InfraManager.BLL.Database.Import;

public class UIDBFieldsQueryBuilder : IFilterEntity<UIDBFields, UIDBFieldsFilter>,
    ISelfRegisteredService<IFilterEntity<UIDBFields, UIDBFieldsFilter>>
{
    private readonly IReadonlyRepository<UIDBFields> _repository;

    public UIDBFieldsQueryBuilder(IReadonlyRepository<UIDBFields> repository)
    {
        _repository = repository;
    }

    public IQueryable<UIDBFields> Query(UIDBFieldsFilter filter)
    {
        var query = _repository.With(x=>x.Configuration)
            .Query();

        if (filter.FieldID.HasValue)
            query = query.Where(x => x.FieldID == filter.FieldID);

        if (filter.ConfigurationID.HasValue)
            query = query.Where(x => x.ConfigurationID == filter.ConfigurationID);

        if (!string.IsNullOrEmpty(filter.Value))
            query = query.Where(x => x.Value.Contains(filter.Value));

        return query;
    }
}