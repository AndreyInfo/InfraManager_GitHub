using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    internal class WorkOrderTypeSearcher : JsonCriteriaObjectSearcher<WorkOrderTypeSearchCriteria>
    {
        private readonly IWorkOrderTypeSearchQuery _searchQuery;

        public WorkOrderTypeSearcher(
            IWorkOrderTypeSearchQuery searchQuery,
            IFinder<Setting> settingsFinder,
            IConvertSettingValue<int> valueConverter,
            IPagingQueryCreator paging,
            ICurrentUser currentUser) : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _searchQuery = searchQuery;
        }

        protected override IQueryable<ObjectSearchResult> Query(Guid userId, WorkOrderTypeSearchCriteria searchBy)
        {
            return _searchQuery.Query(userId, searchBy);
        }
    }
}