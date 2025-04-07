using System;
using System.Linq;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Search;

internal class IncidentResultSearcher: JsonCriteriaObjectSearcher<SearchCriteria>
{
    private readonly IIncidentResultSearchQuery _searchQuery;


    public IncidentResultSearcher(
        IIncidentResultSearchQuery searchQuery,
        IFinder<Setting> settingsFinder,
        IConvertSettingValue<int> valueConverter,
        IPagingQueryCreator paging,
        ICurrentUser currentUser)
        : base(settingsFinder, valueConverter, paging, currentUser)
    {
        _searchQuery = searchQuery;
    }

    protected override IQueryable<ObjectSearchResult> Query(Guid userId, SearchCriteria searchBy) =>
        _searchQuery.Query(searchBy);
}