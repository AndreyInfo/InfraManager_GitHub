using System;
using System.Linq;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Search;

internal class ServiceSearcher : JsonCriteriaObjectSearcher<ServiceSearchCriteria>
{
    private readonly IServiceSearchQuery _query;

    public ServiceSearcher(
        IServiceSearchQuery query,
        ICurrentUser currentUser,
        IFinder<Setting> settingsFinder,
        IPagingQueryCreator paging,
        IConvertSettingValue<int> valueConverter)
        : base(settingsFinder, valueConverter, paging, currentUser)
    {
        _query = query;
    }

    protected override IQueryable<ObjectSearchResult> Query(Guid userID, ServiceSearchCriteria searchBy)
    {
        return _query.Query(searchBy, userID);
    }
}