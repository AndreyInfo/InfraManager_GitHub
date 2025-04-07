using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    internal class OrganizationSearcher : JsonCriteriaObjectSearcher<OrganizationSearchCriteria>
    {
        private readonly IOrganizationSearchQuery _query;

        public OrganizationSearcher(
            IOrganizationSearchQuery query,
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
            OrganizationSearchCriteria searchBy)
        {
            return _query.Query(searchBy, userId);
        }
    }
}
