using System;
using System.Linq;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.KnowledgeBase;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Search
{
    internal class TagSearcher : JsonCriteriaObjectSearcher<SearchCriteria>
    {
        private readonly ITagSearchQuery _query;

        public TagSearcher(
            ITagSearchQuery query,
            IFinder<Setting> settingsFinder,
            IConvertSettingValue<int> valueConverter,
            IPagingQueryCreator paging, ICurrentUser currentUser)
            : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _query = query;
        }

        protected override IQueryable<ObjectSearchResult> Query(Guid userId, SearchCriteria searchBy)
        {
            return _query.Query(searchBy);
        }
    }
}