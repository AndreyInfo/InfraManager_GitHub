using InfraManager.DAL;
using InfraManager.DAL.Settings;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    internal class SplitterSettingsBLL : ISplitterSettingsBLL, ISelfRegisteredService<ISplitterSettingsBLL>
    {
        private readonly IRepository<WebUserSplitterSettings> _webUserSplitterSettings;
        private readonly IMemoryCache _memoryCache;
        private readonly IFinder<WebUserSplitterSettings> _webUserSplitterSettingsFinder;
        private readonly IUnitOfWork _saveChangesCommand;

        public SplitterSettingsBLL(IMemoryCache memoryCache,
            IFinder<WebUserSplitterSettings> webUserSplitterSettingsFinder,
            IRepository<WebUserSplitterSettings> webUserSplitterSettings,
            IUnitOfWork saveChangesCommand)
        {
            _memoryCache = memoryCache;
            _webUserSplitterSettingsFinder = webUserSplitterSettingsFinder;
            _webUserSplitterSettings = webUserSplitterSettings;
            _saveChangesCommand = saveChangesCommand;
        }

        public Task<int> GetAsync(Guid userId, string name)
        {
            return _memoryCache.GetOrCreateAsync(
                GetChacheKey(userId, name),
                cacheEntry => GetSplitterSettings(userId, name));
        }

        public async Task SetAsync(Guid userId, string name, int distance)
        {
            var splitterSettings = await _webUserSplitterSettingsFinder
                .FindAsync(new object[] { userId, name });
            if (splitterSettings == null)
            {
                splitterSettings = new WebUserSplitterSettings(userId,name)
                {
                    Distance = distance
                };
                _webUserSplitterSettings.Insert(splitterSettings);
            }
            else
            {
                splitterSettings.Distance = distance;
            }
            await _saveChangesCommand.SaveAsync();
            _memoryCache.Remove(GetChacheKey(userId, name));
        }

        private async Task<int> GetSplitterSettings(Guid userId, string name)
        {
            var splitterSettings = await _webUserSplitterSettingsFinder.FindAsync(new object[] { userId, name });
            return splitterSettings == null ? default(int) : splitterSettings.Distance;
        }

        private string GetChacheKey(Guid userId, string name)
        {
            return $"SplitterSettings_UserID_{userId}_Name_{name}";
        }
    }
}
