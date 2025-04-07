using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    internal class SubdivisionNoTozSearcher : JsonCriteriaObjectSearcher<SubdivisionSearchCriteria>
    {
        private readonly ISubdivisionSearchQuery _query;

        public SubdivisionNoTozSearcher(
            ISubdivisionSearchQuery query,
            IFinder<Setting> settingsFinder,
            IConvertSettingValue<int> valueConverter,
            IPagingQueryCreator paging,
            ICurrentUser currentUser)
            : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _query = query;
        }

        protected override IQueryable<ObjectSearchResult> Query(
            Guid userId,
            SubdivisionSearchCriteria searchBy)
        {
            return _query.Query(
                new SubdivisionSearchCriteria
                {
                    Text = searchBy.Text,
                    UserId = null
                }, 
                userId);
        }
    }
}
