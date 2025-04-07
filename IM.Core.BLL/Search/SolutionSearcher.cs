using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    internal class SolutionSearcher: JsonCriteriaObjectSearcher<SolutionByNameCriteria>
    {
        private readonly ISolutionSearchQuery _solutionSearch;
        public SolutionSearcher(IFinder<Setting> settingsFinder, IConvertSettingValue<int> valueConverter, IPagingQueryCreator paging, ICurrentUser currentUser
            , ISolutionSearchQuery solutionSearch) 
            : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _solutionSearch = solutionSearch;
        }

        protected override IQueryable<ObjectSearchResult> Query(Guid userId, SolutionByNameCriteria searchBy)
             => _solutionSearch.Query(searchBy);
    }
}
