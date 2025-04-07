using System;
using System.Linq;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Search;

internal class UtilizerSearcher : JsonCriteriaObjectSearcher<UtilizerSearchCriteria>
{
    private readonly IUtilizerSearchQuery _searchQuery;

    public UtilizerSearcher(
        IUtilizerSearchQuery searchQuery,
        IFinder<Setting> settingsFinder,
        IConvertSettingValue<int> valueConverter,
        IPagingQueryCreator paging,
        ICurrentUser currentUser)
        : base(settingsFinder, valueConverter, paging, currentUser)
    {
        _searchQuery = searchQuery;
    }

    protected override IQueryable<ObjectSearchResult> Query(Guid userId, UtilizerSearchCriteria searchBy)
    {
        return _searchQuery.Query(searchBy, userId);
    }
}