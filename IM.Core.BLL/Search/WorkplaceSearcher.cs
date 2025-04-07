using System;
using System.Linq;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Search;

internal class WorkplaceSearcher : JsonCriteriaObjectSearcher<WorkplaceSearchCriteria>
{
    private readonly IWorkplaceSearchQuery _searchQuery;

    public WorkplaceSearcher(
        IWorkplaceSearchQuery searchQuery,
        IFinder<Setting> settingsFinder,
        IConvertSettingValue<int> valueConverter,
        IPagingQueryCreator paging,
        ICurrentUser currentUser)
        : base(settingsFinder, valueConverter, paging, currentUser)
    {
        _searchQuery = searchQuery;
    }

    protected override IQueryable<ObjectSearchResult> Query(Guid userId, WorkplaceSearchCriteria searchBy)
    {
        return _searchQuery.Query(searchBy, userId);
    }
}