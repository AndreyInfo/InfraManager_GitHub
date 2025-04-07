using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Settings;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Search
{
    internal abstract class JsonCriteriaObjectSearcher<TSearchCriteria> : IObjectSearcher
    {
        private readonly IFinder<Setting> _settingsFinder; // TODO: Should be a settingBLL that stores settings in cache
        private readonly IConvertSettingValue<int> _valueConverter;
        private readonly IPagingQueryCreator _paging;
        private readonly ICurrentUser _currentUser;

        protected JsonCriteriaObjectSearcher(
            IFinder<Setting> settingsFinder, 
            IConvertSettingValue<int> valueConverter,
            IPagingQueryCreator paging,
            ICurrentUser currentUser)
        {
            _settingsFinder = settingsFinder;
            _valueConverter = valueConverter;
            _paging = paging;
            _currentUser = currentUser;
        }

        public async Task<ObjectSearchResult[]> SearchAsync(
            string searchString, 
            CancellationToken cancellationToken = default)
        {
            TSearchCriteria searchCriteria;

            try
            {
                searchCriteria = JsonConvert.DeserializeObject<TSearchCriteria>(searchString);
            }
            catch(Exception error)
            {
                throw new InvalidObjectException($"Failed to deserialized search criteria due to error: {error.Message}");
            }
            var take = await TakeLimitUsersAsync(cancellationToken);

            using (var transaction = TransactionScopeCreator.ReadDirty())
            {
                var query = Query(_currentUser.UserId, searchCriteria);
                var paging = _paging.Create(query.OrderBy(x => x.FullName));

                return await paging.PageAsync(skip: 0, take, cancellationToken);
            }
        }

        protected abstract IQueryable<ObjectSearchResult> Query(Guid userID, TSearchCriteria searchBy);

        protected async virtual Task<int> TakeLimitUsersAsync(CancellationToken cancellationToken = default)
        {
            var maxQuantitySetting = await _settingsFinder.FindAsync(SystemSettings.ObjectSearcherResultCount, cancellationToken);
            return _valueConverter.Convert(maxQuantitySetting.Value);
        }
    }
}
