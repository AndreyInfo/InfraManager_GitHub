using InfraManager.DAL;
using InfraManager.DAL.Settings;
using InfraManager.ResourcesArea;
using InfraManager.WebApi.Contracts.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    internal class SettingsBLL : ISettingsBLL, ISelfRegisteredService<ISettingsBLL>
    {
        private const string CacheKey = "settings";

        private readonly IRepository<Setting> _repository;
        private readonly IFinder<Setting> _finder;
        private readonly ILogger<SettingsBLL> _logger;
        private readonly IMemoryCache _cache;
        private readonly IServiceMapper<SystemSettings, IConvertSettingValue> _converters;
        private readonly IUnitOfWork _unitOfWork;

        public SettingsBLL(
            IRepository<Setting> repository,
            IFinder<Setting> finder,
            ILogger<SettingsBLL> logger,
            IMemoryCache cache,
            IServiceMapper<SystemSettings, IConvertSettingValue> converters,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _finder = finder;
            _logger = logger;
            _cache = cache;
            _converters = converters;
            _unitOfWork = unitOfWork;
        }

        public async Task<object> ConvertValueAsync(SystemSettings key, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"Setting {key} is requesting.");
            var value = await GetValueAsync(key, cancellationToken);

            return ConvertElseThrow(value, key);
        }

        private object ConvertElseThrow(byte[] value, SystemSettings key) =>
            _converters.HasKey(key) ? _converters.Map(key).Convert(value) : ThrowNotFoundException(key);

        public byte[] GetValue(SystemSettings key)
        {
            _logger.LogTrace($"Setting {key} is requesting (syncronous).");
            return _cache.GetOrCreate(CacheKey, entry => LoadSettings())[key];
        }

        public async Task<byte[]> GetValueAsync(SystemSettings key, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"Setting {key} is requesting.");
            var settings = await GetSettingsAsync(cancellationToken);
            if (settings.ContainsKey(key))
            {
                return settings[key];
            }
            return null;
        }

        private Task<Dictionary<SystemSettings, byte[]>> GetSettingsAsync(CancellationToken cancellationToken = default)
        {
            return _cache.GetOrCreateAsync(CacheKey, entry => LoadSettingsAsync(cancellationToken));
        }

        private async Task<Dictionary<SystemSettings, byte[]>> LoadSettingsAsync(CancellationToken cancellationToken = default)
        {
            var allSettings = await _repository.ToArrayAsync(cancellationToken);
            _logger.LogInformation($"A set of {allSettings.Length} settings loaded from data source asyncronously.");
            return allSettings.ToDictionary(x => x.Id, x => x.Value);
        }

        private Dictionary<SystemSettings, byte[]> LoadSettings()
        {
            var allSettings = _repository.ToArray();
            _logger.LogInformation($"A set of {allSettings.Length} settings loaded from data source syncronosly.");
            return allSettings.ToDictionary(x => x.Id, x => x.Value);
        }

        public async Task<SettingDetails> GetAsync(SystemSettings key, CancellationToken cancellationToken = default)
        {
            var value = await GetValueAsync(key, cancellationToken);

            return new SettingDetails
            {
                Setting = key,
                RawValue = value, // Workflow -> Settings -> FromApi зависит от RawValue 
                Value = ConvertElseThrow(value, key)
            };
        }

        public async Task<SettingDetails> SetAsync(SystemSettings key, SettingData data, CancellationToken cancellationToken = default)
        {           
            // TODO: Из-за другой ошибки проектирования мы не можем тут проверять права пользователя
            // В системе дыра в безопасности (любой пользователь может отредактировать какую угодно настройку)
            // и сохранять эту дыру в безопасности является требованием к сервису.
            var setting = await _finder.FindAsync(key, cancellationToken);

            var value = ConvertValueBackOrRaiseError(key, data.Value) ?? Array.Empty<byte>();

            if (setting == null)
            {
                setting = new Setting(key, value);
                _repository.Insert(setting);               
            }
            setting.Value = value;

            await _unitOfWork.SaveAsync(cancellationToken);
            _cache.Remove(CacheKey);

            return new SettingDetails
            {
                Setting = key,
                Value = data.Value ?? ConvertElseThrow(value, key)
            };
        }

        private ObjectNotFoundException ThrowNotFoundException(SystemSettings key) 
            => throw new ObjectNotFoundException($"In type {nameof(SettingDetails)} : converter for {key.ToString()} not found.");
        
        private byte[] ConvertValueBackOrRaiseError(SystemSettings key, object value)
        {
            if (value == null)
            {
                return null;
            }

            if (!_converters.HasKey(key))
            {
                throw new InvalidObjectException($"Изменение параметра по значению Value не поддерживается. Используйте RawValue.");
            }

            try
            {
                return _converters.Map(key).ConvertBack(value);
            }
            catch(InvalidOperationException) // тип переданного значения не конвертируется в тип значения настройки
            {
                throw new InvalidObjectException(Resources.InvalidSettingValue);
            }
            catch(OverflowException) // переданное значение конвертируется в тип значения настройки с переполнением
            {
                throw new InvalidObjectException(Resources.InvalidSettingValue);
            }
        }
    }
}
