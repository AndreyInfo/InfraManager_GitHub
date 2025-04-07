using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Calls;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    internal class CallSummarySearcher : JsonCriteriaObjectSearcher<CallSummarySearchCriteria>
    {
        private readonly ICallSummarySearchQuery _callSummarySearchQuery;

        public CallSummarySearcher(
                    IFinder<Setting> settingsFinder,
                    IConvertSettingValue<int> valueConverter,
                    IPagingQueryCreator paging,
                    ICurrentUser currentUser,
                    ICallSummarySearchQuery callSummarySearchQuery)
            : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _callSummarySearchQuery = callSummarySearchQuery;
        }

        protected override IQueryable<ObjectSearchResult> Query(Guid userId, CallSummarySearchCriteria searchBy)
        {
            return _callSummarySearchQuery.Query(searchBy, userId);
        }
    }
}
