using System;
using System.Linq;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Search;

internal class RoomSearcher : JsonCriteriaObjectSearcher<RoomSearchCriteria>
{
    private readonly IRoomSearchQuery _searchQuery;

    public RoomSearcher(
        IRoomSearchQuery searchQuery,
        IFinder<Setting> settingsFinder,
        IConvertSettingValue<int> valueConverter,
        IPagingQueryCreator paging,
        ICurrentUser currentUser)
        : base(settingsFinder, valueConverter, paging, currentUser)
    {
        _searchQuery = searchQuery;
    }

    protected override IQueryable<ObjectSearchResult> Query(Guid userId, RoomSearchCriteria searchBy)
    {
        return _searchQuery.Query(searchBy, userId);
    }
}