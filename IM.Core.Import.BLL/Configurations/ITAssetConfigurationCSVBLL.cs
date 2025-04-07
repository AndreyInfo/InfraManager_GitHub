using AutoMapper;
using IM.Core.Import.BLL.Interface.Configurations;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import.ITAsset;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IM.Core.Import.BLL.Configurations;
internal class ITAssetConfigurationCSVBLL : IITAssetConfigurationCSVBLL, ISelfRegisteredService<IITAssetConfigurationCSVBLL>
{
    private readonly IRepository<ITAssetImportCSVConfiguration> _configurationRepository;
    private readonly IRepository<ITAssetImportCSVConfigurationFieldConcordance> _fieldConcordanceRepository;
    private readonly IRepository<ITAssetImportCSVConfigurationClassConcordance> _classConcordanceRepository;
    private readonly IReadonlyRepository<ITAssetImportCSVConfiguration> _configurationReadonlyRepository;
    private readonly IReadonlyRepository<ITAssetImportCSVConfigurationFieldConcordance> _fieldConcordanceReadonlyRepository;
    private readonly IReadonlyRepository<ITAssetImportCSVConfigurationClassConcordance> _classConcordanceReadonlyRepository;
    private readonly IReadonlyRepository<ProductCatalogType> _productCatalogTypeRepository;

    private readonly IUnitOfWork _saveChangesCommand;
    private readonly IMapper _mapper;
    private readonly ILogger<ITAssetConfigurationCSVBLL> _logger;

    private readonly string[] _availableCategories = new string[]
    {
        "Адаптер",
        "ИТ-Система",
        "Шкафы",
        "Оконечное оборудование",
        "Периферийное устройство",
        "Активное устройство",
        "Расходный материал"
    };

    public ITAssetConfigurationCSVBLL(IRepository<ITAssetImportCSVConfiguration> configurationRepository,
        IRepository<ITAssetImportCSVConfigurationFieldConcordance> fieldConcordanceRepository,
        IRepository<ITAssetImportCSVConfigurationClassConcordance> classConcordanceRepository,
        IUnitOfWork saveChangesCommand,
        IReadonlyRepository<ITAssetImportCSVConfiguration> configurationReadonlyRepository,
        IReadonlyRepository<ITAssetImportCSVConfigurationFieldConcordance> fieldConcordanceReadonlyRepository,
        IReadonlyRepository<ITAssetImportCSVConfigurationClassConcordance> classConcordanceReadonlyRepository,
        IReadonlyRepository<ProductCatalogType> productCatalogTypeRepository,
        IMapper mapper,
        ILogger<ITAssetConfigurationCSVBLL> logger)
    {
        _fieldConcordanceRepository = fieldConcordanceRepository;
        _classConcordanceRepository = classConcordanceRepository;
        _configurationRepository = configurationRepository;
        _saveChangesCommand = saveChangesCommand;
        _configurationReadonlyRepository = configurationReadonlyRepository;
        _fieldConcordanceReadonlyRepository = fieldConcordanceReadonlyRepository;
        _classConcordanceReadonlyRepository = classConcordanceReadonlyRepository;
        _productCatalogTypeRepository = productCatalogTypeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ITAssetImportCSVConfigurationDetails[]> GetConfigurationsAsync(CancellationToken cancellationToken)
    {
        var csvConfigurations = await _configurationReadonlyRepository.ToArrayAsync(cancellationToken);

        var configurationsCSVDetails = _mapper.Map<ITAssetImportCSVConfigurationDetails[]>(csvConfigurations);

        foreach (var config in configurationsCSVDetails)
        {
            var arrayClasses = await _classConcordanceReadonlyRepository
                .ToArrayAsync(x => x.ITAssetImportCSVConfigurationID == config.ID, cancellationToken);
            config.ConfigurationClasses = ParseConfigurationFromArray(arrayClasses);

            var arrayFields = await _fieldConcordanceReadonlyRepository
                .ToArrayAsync(x => x.ITAssetImportCSVConfigurationID == config.ID, cancellationToken);
            config.ConfigurationFields = ParseConfigurationFromArray(arrayFields);
        }

        return configurationsCSVDetails;
    }

    public async Task<ITAssetImportCSVConfigurationDetails> GetConfigurationAsync(Guid id, CancellationToken cancellationToken)
    {
        var csvConfiguration = await _configurationReadonlyRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
        var arrayClasses = await _classConcordanceReadonlyRepository
            .ToArrayAsync(x => x.ITAssetImportCSVConfigurationID == id, cancellationToken);
        var arrayFields = await _fieldConcordanceReadonlyRepository
            .ToArrayAsync(x => x.ITAssetImportCSVConfigurationID == id, cancellationToken);

        var configurationCSVDetails = _mapper.Map<ITAssetImportCSVConfigurationDetails>(csvConfiguration);
        configurationCSVDetails.ConfigurationClasses = ParseConfigurationFromArray(arrayClasses);
        configurationCSVDetails.ConfigurationFields = ParseConfigurationFromArray(arrayFields);

        return configurationCSVDetails;
    }

    public async Task<Guid> SetConfigurationAsync(ITAssetImportCSVConfigurationData configurationData, CancellationToken cancellationToken)
    {
        var CSVConfiguration = _mapper.Map<ITAssetImportCSVConfiguration>(configurationData);

        try
        {
            _configurationRepository.Insert(CSVConfiguration);

            foreach (var cl in GetClassesFromConfiguration(configurationData.ConfigurationClasses, CSVConfiguration.ID))
            {
                cl.ITAssetImportCSVConfigurationID = CSVConfiguration.ID;
                _classConcordanceRepository.Insert(cl);
            }

            foreach (var field in GetFieldsFromConfiguration(configurationData.ConfigurationFields, CSVConfiguration.ID))
            {
                field.ITAssetImportCSVConfigurationID = CSVConfiguration.ID;
                _fieldConcordanceRepository.Insert(field);
            }

            await _saveChangesCommand.SaveAsync(cancellationToken);
            return CSVConfiguration.ID;
        }
        catch (Exception e)
        {
            _logger.LogError("Ошибка создания конфигурации для импорта ит-активов", configurationData.Name, e);
            throw;
        }
    }

    public async Task UpdateConfigurationAsync(Guid id, ITAssetImportCSVConfigurationData configurationData, CancellationToken cancellationToken)
    {
        var classes = GetClassesFromConfiguration(configurationData.ConfigurationClasses, id);

        var oldClasses = await _classConcordanceRepository
            .ToArrayAsync(x => x.ITAssetImportCSVConfigurationID == id, cancellationToken);

        var nullableClasses = oldClasses.ExceptBy(classes.Select(x => x.Field), x => x.Field);

        foreach (var cl in classes)
        {
            var findField = await _classConcordanceRepository
                .FirstOrDefaultAsync(x => x.Field == cl.Field && x.ITAssetImportCSVConfigurationID == id, cancellationToken);

            if (findField is not null)
                findField.Expression = cl.Expression;
            else
                _classConcordanceRepository.Insert(cl);
        }

        foreach (var nullableClass in nullableClasses)
            _classConcordanceRepository.Delete(nullableClass);


        var fields = GetFieldsFromConfiguration(configurationData.ConfigurationFields, id);

        var oldFields = await _fieldConcordanceRepository
            .ToArrayAsync(x => x.ITAssetImportCSVConfigurationID == id, cancellationToken);

        var nullableFields = oldFields.ExceptBy(fields.Select(x => x.Field), x => x.Field);

        foreach (var field in fields)
        {
            var findField = await _fieldConcordanceRepository
                .FirstOrDefaultAsync(x => x.Field == field.Field && x.ITAssetImportCSVConfigurationID == id, cancellationToken);

            if (findField is not null)
                findField.Expression = field.Expression;
            else
                _fieldConcordanceRepository.Insert(field);
        }

        foreach (var nullableField in nullableFields)
            _fieldConcordanceRepository.Delete(nullableField);

        var CSVConfiguration = await _configurationRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

        _mapper.Map(configurationData, CSVConfiguration);
        await _saveChangesCommand.SaveAsync(cancellationToken);
    }

    public async Task DeleteConfigurationAsync(Guid id, CancellationToken cancellationToken)
    {
        var findConfiguration = await _configurationRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
        if (findConfiguration is null)
            return;

        _configurationRepository.Delete(findConfiguration);
        await _saveChangesCommand.SaveAsync(cancellationToken);
    }

    private string ParseConfigurationFromArray(ITAssetImportCSVConfigurationConcordance[] arrayFields)
    {
        Dictionary<string, string> fieldDictionary = new Dictionary<string, string>();
        foreach (var field in arrayFields)
            fieldDictionary.Add(field.Field, field.Expression);

        return JsonConvert.SerializeObject(fieldDictionary);
    }

    private IEnumerable<ITAssetImportCSVConfigurationClassConcordance> GetClassesFromConfiguration(string configuration, Guid id)
    {
        var fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(configuration);
        ITAssetImportCSVConfigurationClassConcordance[] fieldsArray = new ITAssetImportCSVConfigurationClassConcordance[fields.Count];
        if (fields != null)
            for (int i = 0; i < fields.Count; i++)
                fieldsArray[i] = new ITAssetImportCSVConfigurationClassConcordance(id, fields.ElementAt(i).Key, fields.ElementAt(i).Value);

        return fieldsArray;
    }

    private IEnumerable<ITAssetImportCSVConfigurationFieldConcordance> GetFieldsFromConfiguration(string configuration, Guid id)
    {
        var fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(configuration);
        ITAssetImportCSVConfigurationFieldConcordance[] fieldsArray = new ITAssetImportCSVConfigurationFieldConcordance[fields.Count];
        if (fields != null)
            for (int i = 0; i < fields.Count; i++)
                fieldsArray[i] = new ITAssetImportCSVConfigurationFieldConcordance(id, fields.ElementAt(i).Key, fields.ElementAt(i).Value);

        return fieldsArray;
    }

    public async Task<ProductCatalogTypeDetails[]> GetTypesAsync(CancellationToken cancellationToken)
    {
        var productCatalogTypeEntities = await _productCatalogTypeRepository
            .ToArrayAsync(x => _availableCategories.Contains(x.ProductCatalogCategory.Name)
                && x.IsLogical != true && !x.Removed, cancellationToken);

        return _mapper.Map<ProductCatalogTypeDetails[]>(productCatalogTypeEntities);
    }
}
