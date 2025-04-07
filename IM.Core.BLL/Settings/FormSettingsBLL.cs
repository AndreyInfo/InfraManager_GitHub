using InfraManager.DAL;
using InfraManager.DAL.Settings;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    internal class FormSettingsBLL : IFormSettingsBLL, ISelfRegisteredService<IFormSettingsBLL>
    {
        private readonly IRepository<WebUserFormSettings> _repository;
        private readonly IFinder<WebUserFormSettings> _finder;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _saveChanges;
        private readonly IMemoryCache _cache;

        public FormSettingsBLL(
            IRepository<WebUserFormSettings> repository, 
            IFinder<WebUserFormSettings> finder, 
            ICurrentUser currentUser, 
            IUnitOfWork saveChanges,
            IMemoryCache cache)
        {
            _repository = repository;
            _finder = finder;
            _currentUser = currentUser;
            _saveChanges = saveChanges;
            _cache = cache;
        }

        public Task<WebUserFormSettingsModel> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            return _cache.GetOrCreateAsync(
                CacheKey(name),
                entry => GetOrCreateAsync(name, cancellationToken));
        }

        private async Task<WebUserFormSettingsModel> GetOrCreateAsync(string name, CancellationToken cancellationToken = default)
        {
            var settings = await FindAsync(name, cancellationToken);

            return new WebUserFormSettingsModel(
                settings ?? new WebUserFormSettings(_currentUser.UserId, name));
        }

        private ValueTask<WebUserFormSettings> FindAsync(string name, CancellationToken cancellationToken = default)
        {
            return _finder.FindAsync(new object[] { _currentUser.UserId, name }, cancellationToken);
        }

        public async Task<WebUserFormSettingsModel> SetAsync(
            string name,
            WebUserFormSettingsModel model,
            CancellationToken cancellationToken = default)
        {
            var settings = await FindAsync(name, cancellationToken);

            if (settings == null)
            {
                settings = new WebUserFormSettings(_currentUser.UserId, name);
                _repository.Insert(settings);
            }

            settings.X = model.X ?? settings.X;
            settings.Y = model.Y ?? settings.Y;
            settings.Width = model.Width ?? settings.Width;
            settings.Height = model.Height ?? settings.Height;
            settings.Mode = model.Mode ?? settings.Mode;

            await _saveChanges.SaveAsync(cancellationToken);
            _cache.Remove(CacheKey(name));

            return new WebUserFormSettingsModel(settings);
        }

        private string CacheKey(string name) => $"FormSettings_{_currentUser.UserId}_{name}";
    }
}
