using System;
using System.Linq;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Search;

internal class RFCResultSearcher: JsonCriteriaObjectSearcher<SearchCriteria>
{
    private readonly IRFCResultSearchQuery _searchQuery;

    public RFCResultSearcher(
        IRFCResultSearchQuery searchQuery,
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