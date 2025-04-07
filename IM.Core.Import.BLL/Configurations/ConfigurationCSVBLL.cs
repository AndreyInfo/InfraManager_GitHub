using AutoMapper;
using IM.Core.Import.BLL.Interface.Configurations;
using IM.Core.Import.BLL.Interface.Configurations.View;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import.CSV;
using Newtonsoft.Json;
using System.Linq;
using System.Transactions;

namespace IM.Core.Import.BLL.Configurations
{
    internal class ConfigurationCSVBLL : IConfigurationCSVBLL, ISelfRegisteredService<IConfigurationCSVBLL>
    {
        private readonly IReadonlyRepository<UICSVConfiguration> _readOnlyCSVConfigurationRepository;
        private readonly IReadonlyRepository<UICSVIMFieldConcordance> _readOnlyUICSVIMFieldConcordanceRepository;
        private IRepository<UICSVConfiguration> _uiCSVConfigurationRepository;
        private IRepository<UICSVIMFieldConcordance> _uiCSVFieldsRepository;
        private IRepository<UICSVSetting> _uiCSVSettingRepository;

        private readonly IUnitOfWork _saveChangesCommand;

        private readonly IMapper _mapper;
        public ConfigurationCSVBLL(IReadonlyRepository<UICSVConfiguration> readOnlyCSVConfigurationRepository,
            IReadonlyRepository<UICSVIMFieldConcordance> readOnlyUICSVIMFieldConcordanceRepository,
            IRepository<UICSVConfiguration> uiCSVConfigurationRepository,
            IRepository<UICSVIMFieldConcordance> uiCSVFieldsRepository,
            IRepository<UICSVSetting> uiCSVSettingRepository,
            IUnitOfWork saveChangesCommand,
            IMapper mapper)
        {
            _readOnlyCSVConfigurationRepository = readOnlyCSVConfigurationRepository;
            _readOnlyUICSVIMFieldConcordanceRepository = readOnlyUICSVIMFieldConcordanceRepository;
            _uiCSVConfigurationRepository = uiCSVConfigurationRepository;
            _uiCSVFieldsRepository = uiCSVFieldsRepository;
            _uiCSVSettingRepository = uiCSVSettingRepository;   
            _saveChangesCommand = saveChangesCommand;
            _mapper = mapper;
        }
        public async Task<ConfigurationCSVDetails> GetConfigurationAsync(Guid id, CancellationToken cancellationToken)
        {
            var csvConfiguration = await _readOnlyCSVConfigurationRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            var arrayFields = await _readOnlyUICSVIMFieldConcordanceRepository.ToArrayAsync(x => x.CSVConfigurationID == id, cancellationToken);

            var configurationCSVDetails = _mapper.Map<ConfigurationCSVDetails>(csvConfiguration);
            configurationCSVDetails.Configuration = ParseConfigurationFromBitMask(arrayFields);

            return configurationCSVDetails;
        }

        public async Task SetConfigurationAsync(ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken)
        {
            var uiCSVConfiguration = _mapper.Map<UICSVConfiguration>(configurationCSVDetails);

            var fields = ParseBitMaskFromConfiguration(configurationCSVDetails.Configuration, uiCSVConfiguration.ID);

            using (var transaction =
                   new TransactionScope(
                       TransactionScopeOption.Required,
                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                       TransactionScopeAsyncFlowOption.Enabled))
            {
                _uiCSVConfigurationRepository.Insert(uiCSVConfiguration);

                foreach (var field in fields)
                {
                    _uiCSVFieldsRepository.Insert(field);
                }

                await _saveChangesCommand.SaveAsync(cancellationToken);
                transaction.Complete();
            }
        }

        public async Task UpdateConfigurationAsync(Guid id, ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken)
        {
            var csvConfiguration = await _uiCSVConfigurationRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

            var fields = ParseBitMaskFromConfiguration(configurationCSVDetails.Configuration, id);

            var oldFields = await _uiCSVFieldsRepository.ToArrayAsync(x => x.CSVConfigurationID == id, cancellationToken);
            var nullableFields = oldFields.ExceptBy(fields.Select(x => x.IMFieldID), x => x.IMFieldID);

            using (var transaction =
                  new TransactionScope(
                      TransactionScopeOption.Required,
                      new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                      TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var field in fields)
                {
                    var findField = await _uiCSVFieldsRepository.FirstOrDefaultAsync(x => x.IMFieldID == field.IMFieldID && x.CSVConfigurationID == id, cancellationToken);
                    if (findField != null)
                    {
                        findField.Expression = field.Expression;
                    }
                    else
                    {
                        _uiCSVFieldsRepository.Insert(field);
                    }
                }

                foreach (var nullableField in nullableFields)
                {
                    _uiCSVFieldsRepository.Delete(nullableField);
                }

                await _saveChangesCommand.SaveAsync(cancellationToken);
                transaction.Complete();
            }
        }

        public async Task DeleteConfigurationAsync(Guid id, CancellationToken cancellationToken)
        {
            var findConfiguration = await _uiCSVConfigurationRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            if (findConfiguration != null)
            {
                using (var transaction =
                 new TransactionScope(
                     TransactionScopeOption.Required,
                     new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                     TransactionScopeAsyncFlowOption.Enabled))
                {
                    var fields = await _uiCSVFieldsRepository.ToArrayAsync(x => x.CSVConfigurationID == id, cancellationToken);
                    if (fields.Count() > 0)
                    {
                        foreach (var field in fields)
                        {
                            _uiCSVFieldsRepository.Delete(field);
                        }

                    }
                    _uiCSVConfigurationRepository.Delete(findConfiguration);

                    var uiCSVSettings = await _uiCSVSettingRepository.ToArrayAsync(x => x.CSVConfigurationID == findConfiguration.ID);
                    foreach (var setting in uiCSVSettings)
                    {
                        setting.CSVConfigurationID = null;
                    }

                    await _saveChangesCommand.SaveAsync(cancellationToken);
                    transaction.Complete();
                }

            }
        }
        private IEnumerable<UICSVIMFieldConcordance> ParseBitMaskFromConfiguration(string configuration, Guid id)
        {
            var fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(configuration);
            UICSVIMFieldConcordance[] fieldsArray = new UICSVIMFieldConcordance[fields.Count];
            if (fields != null)
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    var imFieldID = (long)Enum.Parse(typeof(ConcordanceObjectType), fields.ElementAt(i).Key);
                    fieldsArray[i] = new UICSVIMFieldConcordance(id, imFieldID, fields.ElementAt(i).Value);
                }
            }
            return fieldsArray;
        }

        private string ParseConfigurationFromBitMask(UICSVIMFieldConcordance[] arrayFields)
        {
            Dictionary<string, string> fieldDictionary = new Dictionary<string, string>();
            foreach (var field in arrayFields)
            {
                var enumField = (ConcordanceObjectType)field.IMFieldID;
                fieldDictionary.Add(enumField.ToString(), field.Expression);
            }

            return JsonConvert.SerializeObject(fieldDictionary);
        }


    }
}
