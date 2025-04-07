using AutoMapper;
using IM.Core.Import.BLL.Interface.Configurations;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import.CSV;
using InfraManager.DAL.Import.ServiceCatalogue;
using InfraManager.ResourcesArea;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ServiceCatalogue;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace IM.Core.Import.BLL.Configurations
{
    public class SCConfigurationCSVBLL : ISCConfigurationCSVBLL, ISelfRegisteredService<ISCConfigurationCSVBLL>
    {
        private readonly IRepository<ServiceCatalogueImportCSVConfiguration> _scImportCSVConfigurationRepository;
        private readonly IRepository<ServiceCatalogueImportCSVConfigurationConcordance> _scImportCSVConfigurationConcordanceRepository;
        private readonly IRepository<ServiceCatalogueImportSetting> _scImportSettingRepository;
        private readonly IReadonlyRepository<ServiceCatalogueImportCSVConfiguration> _sciCSVConfigurationReadonlyRepository;
        private readonly IReadonlyRepository<ServiceCatalogueImportCSVConfigurationConcordance> _sciCSVConfigurationConcordanceReadonlyRepository;

        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IMapper _mapper;
        private readonly ILogger<SCConfigurationCSVBLL> _logger;

        public SCConfigurationCSVBLL(IRepository<ServiceCatalogueImportCSVConfigurationConcordance> scImportCSVConfigurationConcordanceRepository,
            IRepository<ServiceCatalogueImportCSVConfiguration> scImportCSVConfigurationRepository,
            IRepository<ServiceCatalogueImportSetting> scImportSettingRepository,
            IUnitOfWork saveChangesCommand,
            IReadonlyRepository<ServiceCatalogueImportCSVConfiguration> sciCSVConfigurationReadonlyRepository,
            IReadonlyRepository<ServiceCatalogueImportCSVConfigurationConcordance> sciCSVConfigurationConcordanceReadonlyRepository,
            IMapper mapper,
            ILogger<SCConfigurationCSVBLL> logger)
        {
            _scImportCSVConfigurationConcordanceRepository = scImportCSVConfigurationConcordanceRepository;
            _scImportCSVConfigurationRepository = scImportCSVConfigurationRepository;
            _scImportSettingRepository = scImportSettingRepository;
            _saveChangesCommand = saveChangesCommand;
            _sciCSVConfigurationReadonlyRepository = sciCSVConfigurationReadonlyRepository;
            _sciCSVConfigurationConcordanceReadonlyRepository = sciCSVConfigurationConcordanceReadonlyRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task DeleteConfigurationAsync(Guid id, CancellationToken cancellationToken)
        {
            var findConfiguration = await _scImportCSVConfigurationRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            if (findConfiguration is null)
            {
                return;
            }


            _scImportCSVConfigurationRepository.Delete(findConfiguration);
            await _saveChangesCommand.SaveAsync(cancellationToken);

        }

        public async Task<ServiceCatalogueImportCSVConfigurationDetails> GetConfigurationAsync(Guid id, CancellationToken cancellationToken)
        {
            var csvConfiguration = await _sciCSVConfigurationReadonlyRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            var arrayFields = await _sciCSVConfigurationConcordanceReadonlyRepository.ToArrayAsync(x => x.ServiceCatalogueImportCSVConfigurationID == id, cancellationToken);

            var configurationCSVDetails = _mapper.Map<ServiceCatalogueImportCSVConfigurationDetails>(csvConfiguration);
            configurationCSVDetails.Configuration = ParseConfigurationFromArray(arrayFields);

            return configurationCSVDetails;
        }

        private string ParseConfigurationFromArray(ServiceCatalogueImportCSVConfigurationConcordance[] arrayFields)
        {
            Dictionary<string, string> fieldDictionary = new Dictionary<string, string>();
            foreach (var field in arrayFields)
            {
                fieldDictionary.Add(field.Field, field.Expression);
            }

            return JsonConvert.SerializeObject(fieldDictionary);
        }

        public async Task<Guid?> SetConfigurationAsync(ServiceCatalogueImportCSVConfigurationData configurationData, CancellationToken cancellationToken)
        {
            var CSVConfiguration = _mapper.Map<ServiceCatalogueImportCSVConfiguration>(configurationData);

            var fields = GetFieldsFromConfiguration(configurationData.Configuration, CSVConfiguration.ID);


            try
            {
                _scImportCSVConfigurationRepository.Insert(CSVConfiguration);

                foreach (var field in fields)
                {
                    field.ServiceCatalogueImportCSVConfigurationID = CSVConfiguration.ID;
                    _scImportCSVConfigurationConcordanceRepository.Insert(field);
                }

                await _saveChangesCommand.SaveAsync(cancellationToken);

                return CSVConfiguration.ID;
            }
            catch (Exception e)
            {
                _logger.LogError("Ошибка создания конфигурации для импорта сервисов с именем", configurationData.Name, e);
                throw;
            }

        }

        private IEnumerable<ServiceCatalogueImportCSVConfigurationConcordance> GetFieldsFromConfiguration(string configuration, Guid id)
        {
            var fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(configuration);
            ServiceCatalogueImportCSVConfigurationConcordance[] fieldsArray = new ServiceCatalogueImportCSVConfigurationConcordance[fields.Count];
            if (fields != null)
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    fieldsArray[i] = new ServiceCatalogueImportCSVConfigurationConcordance(id, fields.ElementAt(i).Key, fields.ElementAt(i).Value);
                }
            }
            return fieldsArray;
        }

        public async Task UpdateConfigurationAsync(Guid id, ServiceCatalogueImportCSVConfigurationData configurationData, CancellationToken cancellationToken)
        {
            var fields = GetFieldsFromConfiguration(configurationData.Configuration, id);

            var oldFields = await _scImportCSVConfigurationConcordanceRepository.ToArrayAsync(x => x.ServiceCatalogueImportCSVConfigurationID == id, cancellationToken);
            var nullableFields = oldFields.ExceptBy(fields.Select(x => x.Field), x => x.Field);

            foreach (var field in fields)
            {
                var findField = await _scImportCSVConfigurationConcordanceRepository.FirstOrDefaultAsync(x => x.Field == field.Field && x.ServiceCatalogueImportCSVConfigurationID == id, cancellationToken);
                if (findField is not null)
                {
                    findField.Expression = field.Expression;
                }
                else
                {
                    _scImportCSVConfigurationConcordanceRepository.Insert(field);
                }
            }

            foreach (var nullableField in nullableFields)
            {
                _scImportCSVConfigurationConcordanceRepository.Delete(nullableField);
            }

            var CSVConfiguration = await _scImportCSVConfigurationRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            _mapper.Map(configurationData, CSVConfiguration);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<ServiceCatalogueImportCSVConfigurationDetails[]> GetConfigurationsAsync(CancellationToken cancellationToken)
        {
            var csvConfigurations = await _sciCSVConfigurationReadonlyRepository.ToArrayAsync(cancellationToken);

            var configurationsCSVDetails = _mapper.Map<ServiceCatalogueImportCSVConfigurationDetails[]>(csvConfigurations);

            foreach (var config in configurationsCSVDetails)
            {
                var arrayFields = await _sciCSVConfigurationConcordanceReadonlyRepository.ToArrayAsync(x => x.ServiceCatalogueImportCSVConfigurationID == config.ID, cancellationToken);
                config.Configuration = ParseConfigurationFromArray(arrayFields);
            }

            return configurationsCSVDetails;
        }
    }
}
