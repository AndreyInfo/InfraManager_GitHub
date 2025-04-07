using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    internal class ServiceItemAndAttendanceSearcher : JsonCriteriaObjectSearcher<ServiceItemAndAttendanceSearchCriteria>
    {
        private readonly IServiceItemAndAttendanceSearchQuery _query;

        public ServiceItemAndAttendanceSearcher(
            ICurrentUser currentUser, 
            IServiceItemAndAttendanceSearchQuery query,
            IPagingQueryCreator paging,
            IFinder<Setting> settingsFinder, 
            IConvertSettingValue<int> valueConverter)
             : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _query = query;
        }

        protected override IQueryable<ObjectSearchResult> Query(
            Guid userId,
            ServiceItemAndAttendanceSearchCriteria searchBy)
        {
            return _query.Query(searchBy, userId);
        }
    }
}
