using System;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings.TableFilters
{
    internal class AvailableTableFilterElementsBLL : 
        IAvailableTableFilterElementsBLL, 
        ISelfRegisteredService<IAvailableTableFilterElementsBLL>
    {
        private readonly ICultureProvider _cultureProvider;
        private readonly IServiceMapper<string, IBuildAvailableFilterElements> _builders;
        private readonly IMemoryCache _memoryCache;
        private const int CacheSeconds = 5;

        public AvailableTableFilterElementsBLL(
            ICultureProvider cultureProvider,
            IServiceMapper<string, IBuildAvailableFilterElements> builders, 
            IMemoryCache memoryCache)
        {
            _cultureProvider = cultureProvider;
            _builders = builders;
            _memoryCache = memoryCache;
        }

        public async Task<FilterElementBase[]> GetAllAsync(string view, CancellationToken cancellationToken = default)
        {
            var key = $"AllFilterElements_{view}_{await _cultureProvider.GetCurrentCultureAsync(cancellationToken)}";
            if (_memoryCache.TryGetValue(key, out FilterElementBase[] filters))
                return filters;
            
            filters = await BuildAsync(view, cancellationToken);
            _memoryCache.Set(key, filters, TimeSpan.FromSeconds(CacheSeconds));

            return filters;
        }

        public async Task<FilterElementBase> GetByPropertyNameAsync(string view, string propertyName, CancellationToken cancellationToken = default)
        {
            var allFilterElements = await GetAllAsync(view, cancellationToken);
            return allFilterElements.Single(elem => elem.PropertyName == propertyName);
        }

        private Task<FilterElementBase[]> BuildAsync(string view, CancellationToken cancellationToken = default)
        {
            var builder = _builders.Map(view);

            return builder.BuildAsync(cancellationToken);
        }
    }
}
