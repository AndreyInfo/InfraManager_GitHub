using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Problems;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    internal class ProblemCauseSearcher : JsonCriteriaObjectSearcher<SearchCriteria>
    {
        private readonly IProblemCauseSearchQuery _query;

        public ProblemCauseSearcher(
            IProblemCauseSearchQuery query,
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
            SearchCriteria searchBy)
        {
            return _query.Query(searchBy);
        }
    }
}
