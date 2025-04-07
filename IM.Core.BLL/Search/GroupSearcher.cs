using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    internal class GroupSearcher : JsonCriteriaObjectSearcher<GroupSearchCriteria>
    {
        private readonly IGroupSearchQuery _query;

        public GroupSearcher(
            IFinder<Setting> settingsFinder, 
            IConvertSettingValue<int> valueConverter, 
            IPagingQueryCreator paging, 
            ICurrentUser currentUser,
            IGroupSearchQuery query) 
            : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _query = query;
        }

        protected override IQueryable<ObjectSearchResult> Query(Guid userId, GroupSearchCriteria searchBy)
        {
            return _query.Query(searchBy, userId);
        }
    }
}
