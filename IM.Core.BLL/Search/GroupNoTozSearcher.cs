using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Search
{
    internal class GroupNoTozSearcher: JsonCriteriaObjectSearcher<GroupSearchCriteria>
    {
        private readonly IGroupSearchQuery _query;

        public GroupNoTozSearcher(
            IFinder<Setting> settingsFinder,
            IConvertSettingValue<int> valueConverter,
            IPagingQueryCreator paging,
            ICurrentUser currentUser,
            IGroupSearchQuery query)
            : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _query = query;
        }

        protected override IQueryable<ObjectSearchResult> Query(
            Guid userId,
            GroupSearchCriteria searchBy)
        {
            return _query.Query(searchBy);
        }
    }
}
