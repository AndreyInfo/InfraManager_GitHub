using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Settings;
using Newtonsoft.Json;

namespace InfraManager.BLL.Search
{
    internal class UserActivityTypeSearcher : IObjectSearcher
    {
        private readonly IFinder<Setting> _settingsFinder;
        private readonly IConvertSettingValue<int> _valueConverter;
        private readonly IReadonlyRepository<UserActivityType> _repository;

        public UserActivityTypeSearcher(IFinder<Setting> settingsFinder,
            IConvertSettingValue<int> valueConverter,
            IReadonlyRepository<UserActivityType> repository)
        {
            _settingsFinder = settingsFinder;
            _valueConverter = valueConverter;
            _repository = repository;
        }

        public async Task<ObjectSearchResult[]> SearchAsync(string queryString,
            CancellationToken cancellationToken = default)
        {
            SearchCriteria searchCriteria;
            try
            {
                searchCriteria = JsonConvert.DeserializeObject<SearchCriteria>(queryString);
            }
            catch (Exception error)
            {
                //TODO: добавить локализацию
                throw new InvalidObjectException(
                    $"Failed to deserialized search criteria due to error: {error.Message}");
            }

            var maxQuantitySetting =
                await _settingsFinder.FindAsync(SystemSettings.ObjectSearcherResultCount, cancellationToken);
            var take = _valueConverter.Convert(maxQuantitySetting.Value);

            var allActivities = _repository.With(p => p.Parent).ToArray();
            return allActivities.Where(a => a.Name.Contains(searchCriteria.Text)).OrderBy(a => a.Name)
                .Take(take).Select(a => new ObjectSearchResult
                {
                    ClassID = ObjectClass.UserActivityType,
                    ID = a.ID,
                    FullName = a.Name,
                    Details = a.FullName
                }).ToArray();
        }
    }
}