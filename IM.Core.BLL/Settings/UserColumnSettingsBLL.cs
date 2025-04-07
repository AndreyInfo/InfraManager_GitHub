using InfraManager.DAL;
using InfraManager.DAL.Settings;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    internal class UserColumnSettingsBLL : IUserColumnSettingsBLL, ISelfRegisteredService<IUserColumnSettingsBLL>
    {
        private readonly IRepository<WebUserColumnSettings> _repository;
        private readonly IUnitOfWork _saveChanges;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceMapper<string, IBuildDefaultUserColumnSettings> _defaultSettingsBuilders;
        private readonly IWebUserSettingsBLL _settings;
        private readonly ICurrentUser _currentUser;

        public UserColumnSettingsBLL(
            IRepository<WebUserColumnSettings> repository,
            IUnitOfWork saveChanges,
            IMemoryCache memoryCache,
            IServiceMapper<string, IBuildDefaultUserColumnSettings> defaultSettingsBuilders,
            IWebUserSettingsBLL settings,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _saveChanges = saveChanges;
            _memoryCache = memoryCache;
            _defaultSettingsBuilders = defaultSettingsBuilders;
            _settings = settings;
            _currentUser = currentUser;
        }

        public async Task<ColumnSettings[]> GetAsync(Guid userId, string viewName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ObjectNotFoundException($"ViewName not found");
            }

            var settings = await _settings.GetAsync(userId, cancellationToken);
            var cacheKey = $"UserColumnSettings_{userId}_{viewName}_{settings.CultureName}";

            return await _memoryCache.GetOrCreateAsync(
                cacheKey,
                cacheEntry => GetOrCreateDefault(userId, viewName, settings.CultureName, cancellationToken));
        }

        public Task<ColumnSettings[]> GetAsync(
            string viewName,
            CancellationToken cancellationToken = default)
        {
            return GetAsync(_currentUser.UserId, viewName, cancellationToken);
        }

        private async Task<ColumnSettings[]> GetOrCreateDefault(
            Guid userId,
            string viewName,
            string locale,
            CancellationToken cancellationToken = default)
        {
            var columns = await _repository
                .ToArrayAsync(
                    x => x.UserId == userId && x.ListName == viewName,
                    cancellationToken);
            var defaultColumns = await _defaultSettingsBuilders
                .Map(viewName)
                .BuildAsync(locale);
            var result = new List<ColumnSettings>(defaultColumns);

            var defaultColumnsNames = defaultColumns.Select(x => x.MemberName);
            columns = columns.Where(col => col.IsVirtual || defaultColumnsNames.Contains(col.CleanMemberName)).ToArray();

            result.AddRange(
                columns
                    .Where(col => col.IsVirtual)
                    .Select(col => new ColumnSettings(col.CleanMemberName)));

            foreach (var columnPair in defaultColumns
                .Join(
                    columns,
                    x => x.MemberName,
                    x => x.CleanMemberName,
                    (DefaultColumn, SavedColumn) => new { DefaultColumn, SavedColumn }))
            {
                columnPair.DefaultColumn.UpdateFrom(columnPair.SavedColumn);
            }

            return result.ToArray();
        }

        public async Task SetAsync(string viewName, ColumnSettings[] columns, CancellationToken cancellationToken = default)
        {
            var userId = _currentUser.UserId;
            var settings = await _settings.GetAsync(userId, cancellationToken);
            var currentColumns = await _repository
                .ToArrayAsync(
                    x => x.UserId == _currentUser.UserId && x.ListName == viewName,
                    cancellationToken);
            var currentColumnsDictionary = currentColumns.ToDictionary(x => x.CleanMemberName);
            var defaultColumns = await _defaultSettingsBuilders
                .Map(viewName)
                .BuildAsync(settings.CultureName);

            var currentDefaultColumnSettingsPair = from defaultColumnSettings in defaultColumns
                                                   join currentColumnSettings in currentColumnsDictionary.Values on defaultColumnSettings.MemberName equals currentColumnSettings.CleanMemberName
                                                   into currentDefaultColumnSettings
                                                   from currentColumnSettings in currentDefaultColumnSettings.DefaultIfEmpty()
                                                   select new
                                                   {
                                                       defaultColumnSettings,
                                                       currentColumnSettings
                                                   };
            foreach (var column in currentDefaultColumnSettingsPair.Where(col => col.currentColumnSettings == null))
            {
                var webUserColumnSettings = column.defaultColumnSettings.CreateWebUserColumnSettings(userId, viewName);
                _repository.Insert(webUserColumnSettings);
                currentColumnsDictionary.Add(
                    column.defaultColumnSettings.MemberName, 
                    webUserColumnSettings);
            }

            foreach (var column in columns)
            {
                var webUserColumnSettings = column.CreateWebUserColumnSettings(userId, viewName);
                if (!currentColumnsDictionary.ContainsKey(webUserColumnSettings.CleanMemberName) && !webUserColumnSettings.IsVirtual)
                {
                    throw new Exception($"UserID_{userId} memberName_{webUserColumnSettings.CleanMemberName} is not found.");
                }
                else if (!currentColumnsDictionary.ContainsKey(webUserColumnSettings.CleanMemberName))
                {
                    _repository.Insert(webUserColumnSettings);
                }
                else
                {
                    var currentColumn = currentColumnsDictionary[webUserColumnSettings.CleanMemberName];
                    currentColumn.Order = webUserColumnSettings.Order;
                    currentColumn.Width = webUserColumnSettings.Width;
                    currentColumn.Visible = webUserColumnSettings.Visible;
                    currentColumn.SortAsc = webUserColumnSettings.SortAsc;
                    currentColumn.CtrlSortAsc = webUserColumnSettings.CtrlSortAsc;
                }
            }
            await _saveChanges.SaveAsync(cancellationToken);
            _memoryCache.Remove($"UserColumnSettings_{userId}_{viewName}_{settings.CultureName}");
        }
    }
}
