using InfraManager.ResourcesArea;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings.TableFilters
{
    internal class TableFilterOperationsBLL
        : ITableFilterOperationsBLL,
        ISelfRegisteredService<ITableFilterOperationsBLL>
    {
        private readonly ICultureProvider _cultureProvider;
        private readonly ICurrentUser _currentUser;
        private readonly IMemoryCache _memoryCache;

        public TableFilterOperationsBLL(
            ICultureProvider cultureProvider,
            ICurrentUser currentUser,
            IMemoryCache memoryCache)
        {
            _cultureProvider = cultureProvider;
            _currentUser = currentUser;
            _memoryCache = memoryCache;
        }

        private static Dictionary<FilterTypes, Type> _mapping =
            new Dictionary<FilterTypes, Type>
            {
                { FilterTypes.DatePick, typeof(DateTimeSearchOperation) },
                { FilterTypes.LikeValue, typeof(PatternSearchOperation) },
                { FilterTypes.SelectorMultiple, typeof(MultiSelectSearchOperation) },
                { FilterTypes.SimpleValue, typeof(SimpleValueSearchOperation) },
                { FilterTypes.SliderRange, typeof(RangeSearchOperation) }
            };

        public Task<LookupListItem<byte>[]> GetByElementTypeAsync(
            FilterTypes type, 
            CancellationToken cancellationToken = default)
        {
            return _memoryCache.GetOrCreateAsync(
                $"TableFilterElementOperations_{type}_{_cultureProvider.GetCurrentCulture()}",
                async _ => await CreateAsync(type));
        }

        private async Task<LookupListItem<byte>[]> CreateAsync(FilterTypes type)
        {
            var uiCulture = await _cultureProvider.GetUiCultureInfoAsync();
            var operationType = _mapping[type];

            return operationType.GetFields()
                .Where(f => f.HasAttribute<DisplayAttribute>())
                .Select(
                    f => new LookupListItem<byte>
                    {
                        ID = (byte)f.GetValue(null),
                        Name = f.GetAttribute<DisplayAttribute>().GetResourceValue(uiCulture.Name)
                    })
                .ToArray();
        }
    }
}
