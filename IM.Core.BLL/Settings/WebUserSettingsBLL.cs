using System;
using System.Threading.Tasks;
using InfraManager.DAL;
using WebUserSettingsEntity = InfraManager.DAL.Settings.WebUserSettings;
using Microsoft.Extensions.Caching.Memory;
using InfraManager.BLL.AccessManagement;
using System.Threading;
using System.Reflection;
using System.Linq;
using AutoMapper;

namespace InfraManager.BLL.Settings
{
    internal class WebUserSettingsBLL : 
        IWebUserSettingsBLL, 
        ICultureProvider,
        ISelfRegisteredService<IWebUserSettingsBLL>,
        ISelfRegisteredService<ICultureProvider>
    {
        #region .ctor and dependencies (fields)

        private readonly ICurrentUser _currentUser;
        private readonly IUserAccessBLL _userAccess;
        private readonly IRepository<WebUserSettingsEntity> _repository;
        private readonly IMemoryCache _memoryCache;
        private readonly IFinder<WebUserSettingsEntity> _finder;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IMapper _mapper;
        private readonly IDefaultClientCultureProvider _defaultClientCultureProvider;

        public WebUserSettingsBLL(
            ICurrentUser currentUser,
            IUserAccessBLL userAccess,
            IFinder<WebUserSettingsEntity> finder,
            IMemoryCache memoryCache,
            IRepository<WebUserSettingsEntity> repository,
            IUnitOfWork saveChangesCommand,
            IMapper mapper,
            IDefaultClientCultureProvider defaultClientCultureProvider)
        {
            _currentUser = currentUser;
            _userAccess = userAccess;
            _finder = finder;
            _memoryCache = memoryCache;
            _repository = repository;
            _saveChangesCommand = saveChangesCommand;
            _mapper = mapper;
            _defaultClientCultureProvider = defaultClientCultureProvider;
        }

        #endregion

        #region ICultureProvider

        public string GetCurrentCulture()
        {
            if (!TryGetValue(out var settings))
            {
                settings = _finder.Find(_currentUser.UserId);
            }

            return GetCulture(settings);
        }

        public async Task<string> GetCurrentCultureAsync(CancellationToken cancellationToken = default)
        {
            if (!TryGetValue(out var settings))
            {
                settings = await _finder.FindAsync(_currentUser.UserId, cancellationToken);
            }

            return GetCulture(settings);
        }

        private bool TryGetValue(out WebUserSettingsEntity settings) =>
            _memoryCache.TryGetValue(GetCacheKey(_currentUser.UserId), out settings);

        private string GetCulture(WebUserSettingsEntity settings) => settings?.CultureName ?? GetDefaultCulture();

        private string GetDefaultCulture()
        {
            return _defaultClientCultureProvider.ClientCultureName;
        }

        #endregion

        #region IWebUserSettingsBLL

        public async ValueTask<WebUserSettings> GetAsync(Guid userID, CancellationToken cancellationToken = default)
        {
            var settings = await _memoryCache.GetOrCreateAsync(
                GetCacheKey(userID),
                cacheEntry => GetDetailsAsync(userID));

            // Если пользователь потерял доступ к последнему списку раздела Задачи, в котором он был, то вернем дефолтный для него
            if (!await _userAccess.ViewIsGrantedAsync(userID, settings.ViewNameSD, cancellationToken))
            {
                settings.ViewNameSD = await DefaultServiceDescViewNameAsync(userID, cancellationToken);
            }

            return settings;
        }

        public async Task SetAsync(Guid userID, WebUserSettings data, CancellationToken cancellationToken = default)
        {
            var entity = await _finder.FindAsync(userID, cancellationToken);

            if (entity == null)
            {
                entity = await CreateDefaultAsync(userID, cancellationToken);
                _repository.Insert(entity);
            }            

            if (!string.IsNullOrWhiteSpace(data.AssetFiltrationField))
            {
                entity.SetTreeSettings(
                    string.IsNullOrWhiteSpace(data.AssetFiltrationObjectID)
                        ? null
                        : new Guid(data.AssetFiltrationObjectID),
                    data.AssetFiltrationObjectClassID ?? default,
                    data.AssetFiltrationObjectName,
                    data.AssetFiltrationTreeType ?? default,
                    data.AssetFiltrationField);
            }

            _mapper.Map(data, entity);

            await _saveChangesCommand.SaveAsync(cancellationToken);
            ClearCache(userID);
        }

        private async Task<WebUserSettingsEntity> CreateDefaultAsync(Guid userID, CancellationToken cancellationToken = default)
        {
            return new WebUserSettingsEntity(userID)
            {
                CultureName = GetDefaultCulture(),
                UseCompactMenuOnly = true,
                ViewNameSD = await DefaultServiceDescViewNameAsync(userID, cancellationToken),
                ViewNameAsset = ListView.HardwareList,
                ViewNameFinance = ListView.FinanceActivesRequestList,
                IncomingCallProcessing = false,
                TimeSheetFilter = 7,
                ListViewCompactMode = false,
                ListViewGridLines = true,
                ListViewMulticolor = false,
            };
        }

        private async Task<WebUserSettings> GetDetailsAsync(Guid userID, CancellationToken cancellationToken = default)
        {
            var userSettings = await _finder.FindAsync(userID, cancellationToken);
            var isDefault = userSettings == null;
            userSettings = userSettings ?? await CreateDefaultAsync(userID, cancellationToken);

            return Convert(userSettings, isDefault);
        }

        private WebUserSettings Convert(WebUserSettingsEntity entity, bool isDefault)
        {
            var details = _mapper.Map<WebUserSettings>(entity);
            details.IsDefault = isDefault;

            return details;
        }

        private static string GetCacheKey(Guid userId) => $"webUserSettings_{userId}";

        public async Task ResetAsync(Guid userID, CancellationToken cancellationToken = default)
        {
            var currentSettings = await _finder.FindAsync(userID, cancellationToken);
            if (currentSettings != null)
            {
                _repository.Delete(currentSettings);
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
            ClearCache(userID);
        }

        private void ClearCache(Guid userID) => _memoryCache.Remove(GetCacheKey(userID));

        private async Task<string> DefaultServiceDescViewNameAsync(Guid userID, CancellationToken cancellationToken = default)
        {
            foreach(var view in ListView.ServiceDeskViews) 
            {
                if (await _userAccess.ViewIsGrantedAsync(userID, view, cancellationToken))
                {
                    return view;
                }
            }

            return string.Empty;
        }

        #endregion
    }

    
}

