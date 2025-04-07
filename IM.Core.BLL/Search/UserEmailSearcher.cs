using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    internal class UserEmailSearcher : JsonCriteriaObjectSearcher<UserEmailSearchCriteria>
    {
        private readonly IUserEmailSearchQuery _query;

        public UserEmailSearcher(
            IUserEmailSearchQuery query,
            IFinder<Setting> settingsFinder,
            IConvertSettingValue<int> valueConverter,
            IPagingQueryCreator paging,
            ICurrentUser currentUser)
            : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _query = query;
        }

        protected override IQueryable<ObjectSearchResult> Query(Guid userId, UserEmailSearchCriteria searchBy)
        {
            return _query.Query(
                new UserEmailSearchCriteria
                {
                    Text = searchBy.Text
                },
                userId);
        }
    }
}
