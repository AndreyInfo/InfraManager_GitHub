using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Database;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Import.OSU;
using InfraManager;
using InfraManager.BLL;
using InfraManager.BLL.Import;
using InfraManager.DAL;
using InfraManager.DAL.Database.Import;
using InfraManager.DAL.Import;
using InfraManager.DAL.Import.CSV;
using InfraManager.ServiceBase.ImportService.DBService;
using InfraManager.ServiceBase.ScheduleService;
using IronPython.Modules;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL.Import.OSU;

internal class ImportDBBLL : ImportDeleteSettings<UIDBSettings>, IImportDBBLL, ISelfRegisteredService<IImportDBBLL>
{
    //todo:сделать общий сервис
    private const ConcordanceObjectType UserMask = ConcordanceObjectType.UserOrganization
                                                   | ConcordanceObjectType.UserOrganizationExternalID
                                                   | ConcordanceObjectType.UserSubdivision
                                                   | ConcordanceObjectType.UserSubdivisionExternalID;
    private readonly IUIDBFieldsBLL _fields;

    private readonly IUIDBSettingsBLL _settings;

    private readonly IUIDBConfigurationBLL _configurationBLL;

    private readonly IScriptData _scriptData;

    private readonly IScriptDataParser<ConcordanceObjectType> _scriptDataParser;
    
    private readonly ILocalLogger<IImportDBBLL> _logger;

    private readonly IUIDBConnectionStringBLL _connectionString;

    private readonly ILoadFromDatabase _loadFromDatabase;


    private readonly IMapper _mapper;

    public ImportDBBLL(IRepository<UIDBSettings> settingsRepository,
        IUIDBFieldsBLL fields,
        IUIDBSettingsBLL settings,
        IMapper mapper,
        IUIDBConfigurationBLL configurationBLL,
        IScriptData scriptData,
        IScriptDataParser<ConcordanceObjectType> scriptDataParser,
        ILocalLogger<IImportDBBLL> protocolLogger,
        IUIDBConnectionStringBLL connectionString,
        ILoadFromDatabase loadFromDatabase, 
        ILocalLogger<IImportDBBLL> logger) : base(settingsRepository)
    {
        _fields = fields;
        _settings = settings;
        _mapper = mapper;
        _configurationBLL = configurationBLL;
        _scriptData = scriptData;
        _scriptDataParser = scriptDataParser;
        _connectionString = connectionString;
        _loadFromDatabase = loadFromDatabase;
        _logger = logger;
        protocolLogger = protocolLogger;
    }

    public async Task<Guid?> GetIDConfiguration(Guid id, CancellationToken cancellationToken)
    {
        var settings = await _settings.DetailsAsync(id, cancellationToken);
        return settings?.DBConfigurationID;
    }

    public async Task SetUISettingAsync(Guid id, Guid? selectedConfiguration, CancellationToken cancellationToken)
    {
        var settingsFromBase = await _settings.DetailsAsync(id, cancellationToken);
        if (settingsFromBase != null)
        {
            settingsFromBase.DBConfigurationID = selectedConfiguration;
            var input = _mapper.Map<UIDBSettingsData>(settingsFromBase);
            await _settings.UpdateAsync(id, input, cancellationToken);
        }
        else
        {
            var settings = new UIDBSettingsData()
            {
                ID = id,
                DBConfigurationID = selectedConfiguration,
            };
            await _settings.AddAsync(settings, cancellationToken);
        }
    }

    public async Task<ImportModel?[]> GetImportModelsAsync(ImportTaskRequest importDetails,
        UISetting settings,
        IProtocolLogger protocolLogger,
        Func<UISetting?, IProtocolLogger, CancellationToken, Task> verify,
        CancellationToken cancellationToken)
    {
       
        protocolLogger.Information("Подключен источник данных: DB");
        protocolLogger.AddInputData(InfraManager.ServiceBase.ImportService.Log.ImportInputType.DB);
        //todo:рефакторинг!!!
        await verify(settings, protocolLogger, cancellationToken);
        
        UIDBSettingsOutputDetails settingsDB = default;
        try
        {
            settingsDB = await _settings.DetailsAsync(settings.ID, cancellationToken) ??
                         throw new ObjectNotFoundException("Не найдены настройки");
        }
        catch (Exception e)
        {
            _logger.Information($"Не удалось загрузить настройки базы данных для Settings {settings.ID}");
            throw;
        }
        
        if (!settingsDB.DBConfigurationID.HasValue)
            throw new ObjectNotFoundException("Не указана конфигурация");

        //конфигурация
        var configuration = await _configurationBLL.DetailsAsync(settingsDB.DBConfigurationID.Value, cancellationToken);
        //поля
        var fields = await _fields.GetDetailsArrayAsync(new UIDBFieldsFilter()
            {
                ConfigurationID = configuration.ID,
            },
            cancellationToken);

        //поля оргструктуры
        var importFields = fields.ToDictionary(x => x.FieldName, x => x.Value);


        var importTypes = new List<ImportModel>();

        Func<KeyValuePair<string,string>,ScriptDataDetails<ConcordanceObjectType>> init = x =>
            new ScriptDataDetails<ConcordanceObjectType>()
            {
                FieldEnum = System.Enum.Parse<ConcordanceObjectType>(x.Key),
                Script = x.Value, 
            };
        var concordanceObjectType = (ConcordanceObjectType) settings.ObjectType;
        if (settings.UpdateLocation && ((LocationTypeEnum) settings.LocationMode) == LocationTypeEnum.User)
            concordanceObjectType |= ConcordanceObjectType.UserWorkplace;
        var typeObject = (ObjectType) settings.ObjectType;
        //todo:перенести в importdata
        if (concordanceObjectType.HasFlag(ConcordanceObjectType.User))
            concordanceObjectType |= UserMask;
        if (typeObject.HasFlag(ObjectType.UserManager))
            concordanceObjectType |= ConcordanceObjectType.UserManager;
        var fieldsData = _scriptData.GetScriptDataDetailsEnumerable(importFields,
            concordanceObjectType, init);
        ImportHelper.PrintConfiguration(fieldsData,this._logger);

        var databaseName = settingsDB.DatabaseName;
        
        var (organizationFieldData, organizationFieldNames) =
            ScriptDataDetailsList(fieldsData, ConcordanceObjectType.Organization);
        if (!string.IsNullOrWhiteSpace(configuration.OrganizationTableName))
            await LoadTableAsync(organizationFieldNames, organizationFieldData, importTypes,
                configuration.OrganizationTableName, ImportTypeEnum.Organization, databaseName, cancellationToken);

        var (subdivisionFieldData, subdivisionFieldNames) =
            ScriptDataDetailsList(fieldsData, ConcordanceObjectType.Subdivision);
        if (!string.IsNullOrWhiteSpace(configuration.SubdivisionTableName))
            await LoadTableAsync(subdivisionFieldNames, subdivisionFieldData, importTypes,
                configuration.SubdivisionTableName, ImportTypeEnum.Subdivision, databaseName, cancellationToken);

        var (userFieldData, userFieldNames) = 
            ScriptDataDetailsList(fieldsData, ConcordanceObjectType.User);
        if (!string.IsNullOrWhiteSpace(configuration.UserTableName))
            await LoadTableAsync(userFieldNames, userFieldData, importTypes, configuration.UserTableName,
                ImportTypeEnum.User, databaseName, cancellationToken);



        return importTypes.ToArray();
    }

    private (List<ScriptDataDetails<ConcordanceObjectType>> organizationFieldData, HashSet<string?>
        organizationFieldNames) ScriptDataDetailsList(IEnumerable<ScriptDataDetails<ConcordanceObjectType>> fieldsData,
            ConcordanceObjectType concordanceObjectType)
    {
        var fieldData = fieldsData.Where(x => x.FieldEnum.HasFlag(concordanceObjectType))
            .ToList();
        var fieldNames = _scriptData.GetScriptRecordFields(fieldData);
        return (fieldData, fieldNames);
    }

    private async Task LoadTableAsync(HashSet<string?> fieldNames,
        IEnumerable<ScriptDataDetails<ConcordanceObjectType>> fieldsData,
        ICollection<ImportModel> importTypes,
        string tableName,
        ImportTypeEnum concordance,
        string databaseName,
        CancellationToken cancellationToken)
    {
        if (!fieldsData.Any())
            return;
        var scripts = _scriptDataParser.GetParserData(fieldsData);
       
        var importModelsAsyncIterator = _loadFromDatabase.ImportModelsAsync(fieldNames,
            tableName, databaseName, cancellationToken);
        await foreach (var tableData in importModelsAsyncIterator)
        {
            _logger.Information("Вычисление полей объекта");
            _logger.Information("Прочитано:");
            _logger.Information(tableData.ToString());
            
            var fieldName = tableData.Cells.Keys.ToArray();
            var values = tableData.Cells.Values.ToArray();
            var objectDictionary = _scriptDataParser.ParseToObjectDictionary(scripts, fieldName, values);
            if (!objectDictionary.Any())
            {
                _logger.Information("Ничего не распарсено. Проигнорированно.");
                continue;
            }
            _logger.Information("Интерпретировано как:");
            var importModel = ImportHelper.GetImportModel(objectDictionary);
            importModel.ImportType = concordance;
            _logger.Information($"Получено {importModel}");

            importTypes.Add(importModel);
            _logger.Information($"Добавлено в список выходных параметров");
        }
    }

    public async Task UpdateUISettingAsync(Guid iD, Guid? selectedConfiguration, CancellationToken cancellationToken)
    {
        await SetUISettingAsync(iD, selectedConfiguration, cancellationToken);
    }

    public async Task<Guid?> GetConfigurationIDBySettingAsync(Guid settingID, CancellationToken cancellationToken)
    {
        return await GetIDConfiguration(settingID, cancellationToken);
    }

    public async Task<IEnumerable<UIIMFieldConcordance>> GetFieldsByConfigurationIDAsync(Guid? idConfiguration,
        CancellationToken token)
    {
        var fields =
            await _fields.GetDetailsArrayAsync(new UIDBFieldsFilter()
            {
                ConfigurationID = idConfiguration,
            }, token);
        return _mapper.Map<UIIMFieldConcordance[]>(fields);
    }
}