using System.Reflection;
using System.Text;
using AutoMapper;
using IM.Core.Import.BLL.Import.Helpers;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Configurations.View;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.OSU;
using IM.Core.Import.BLL.Interface.Import.View;
using IM.Core.Import.BLL.UserImport.Log;
using InfraManager.DAL;
using InfraManager.DAL.Import;
using InfraManager.ResourcesArea;
using InfraManager.ServiceBase.ImportService.LdapModels;
using InfraManager.ServiceBase.ScheduleService;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;


namespace InfraManager.BLL.Import
{
    public class ImportBLL : IImportBLL, ISelfRegisteredService<IImportBLL>
    {
        private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly IServiceMapper<int, IImportBase> _importBaseMapper;
        private readonly IReadonlyRepository<UISetting> _readOnlyUISettingRepository;
        private readonly IRepository<UISetting> _uiSettingsRepository;
        private readonly IImportAnalyzerBLL _analyzer;
        private readonly ILocalLogger<ImportBLL> _logger;
        private readonly IProtocolLogger _protocolLogger;

        private readonly IScheduleServiceWebApi _importToScheduleServiceClientApi;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IPolledObjectConverter _polledObjectConverter;
        private readonly IServiceProvider _serviceProvider;
        private readonly IValidationBLL _validationBLL;

        private readonly IMapper _mapper;

        private readonly List<ObjectType> _ignoredObjectTypes = new()
        {
            ObjectType.SubdivisionParent,
            ObjectType.SubdivisionParentExternalID,
            ObjectType.SubdivisionOrganization,
            ObjectType.SubdivisionOrganizationExternalID,
            ObjectType.UserOrganization,
            ObjectType.UserOrganizationExternalID,
            ObjectType.UserSubdivision,
            ObjectType.UserSubdivisionExternalID
        };


        public ImportBLL(
            IReadonlyRepository<UISetting> readOnlyUISettingRepository,
            IRepository<UISetting> uiSettingsRepository,
            IServiceMapper<int, IImportBase> importBaseMapper,
            IUnitOfWork saveChangesCommand,
            IImportAnalyzerBLL analyzer,
            ILocalLogger<ImportBLL> logger,
            IMapper mapper,
            IScheduleServiceWebApi importToScheduleServiceClientApi,
            IPolledObjectConverter polledObjectConverter,
            IProtocolLogger protocolLogger, IServiceProvider serviceProvider, IValidationBLL validationBLL)
        {
            _readOnlyUISettingRepository = readOnlyUISettingRepository;
            _uiSettingsRepository = uiSettingsRepository;
            _saveChangesCommand = saveChangesCommand;
            _importBaseMapper = importBaseMapper;
            _logger = logger;
            _analyzer = analyzer;
            _mapper = mapper;
            _importToScheduleServiceClientApi = importToScheduleServiceClientApi;
            _polledObjectConverter = polledObjectConverter;
            _protocolLogger = protocolLogger;
            _serviceProvider = serviceProvider;
            _validationBLL = validationBLL;
        }

        public async Task StartImportAsync(ImportTaskRequest importTasksDetails)
        {
            if (_semaphoreSlim.CurrentCount != 1) //не ожидать завершения
                return;

            await _semaphoreSlim.WaitAsync();

            try
            {
                await RunImportAsync(importTasksDetails);
            }
            catch (Exception e)
            {
                _logger.Information("Работа импорта остановлена из-за критической ошибки, смотри лог ошибок");
                _logger.Error(e, e.Message);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private async Task RunImportAsync(ImportTaskRequest importTasksDetails)
        {
           try
            {
                CancellationToken cancellationToken = default; // отмена овязана чтобы импорт не падал
                Exception? exception = null;
                Exception? finallyException = null;
                try
                {
                    var uiSettings =
                        await _readOnlyUISettingRepository.FirstOrDefaultAsync(
                            x => x.ID == importTasksDetails.SettingID,
                            cancellationToken)
                        ?? throw new ObjectNotFoundException(
                            $"Не найден UISetting с ID = {importTasksDetails.SettingID}");
                    
                    _protocolLogger.StartTask(uiSettings.Name, ImportTaskTypeEnum.Users,
                        (Guid) importTasksDetails.SettingID, importTasksDetails.ID);
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        #region Scope init
                        var scopeProvider = scope.ServiceProvider;
                        var importContext = scopeProvider.GetRequiredService<IImportContext>();
                        var importBaseMapper = scopeProvider.GetRequiredService<IServiceMapper<int, IImportBase>>();
                        if (!importBaseMapper.HasKey(uiSettings.ProviderType))
                        {
                            _logger.Information("Тип провайдера не поддерживается");
                            throw new InvalidDataException(
                                $"{nameof(uiSettings.ProviderType)}: Тип провайдера не поддерживается");
                        }
                        var importBase = importBaseMapper.Map(uiSettings.ProviderType);
                        if (importBase == null)
                            throw new NullReferenceException("Не удалось получить провайдер данных");
                        _protocolLogger.Information(
                            $"Запущена задача импорта номер: {importTasksDetails.SettingID},\n название задачи: {uiSettings.Name},\nо задаче {uiSettings.Note}\n  номер задачи планировщика: {importTasksDetails.ID}");

                        var analyzer = scopeProvider.GetRequiredService<IImportAnalyzerBLL>();
                        var contextLogger = scopeProvider.GetRequiredService<ILocalLogger<ImportBLL>>(); 
                        importContext.AdditionalContextLogger = new ProtocolLogAdapter(_protocolLogger);
                        #endregion
                        
                        contextLogger.Information("Загрузка пользователей");
                        //todo:рефакторинг!!!
                        Func<UISetting?, IProtocolLogger, CancellationToken, Task> verify = async (uiSettings, protocolLogger, cancellationToken)=> await Verify(uiSettings, protocolLogger, cancellationToken);
                        
                        ImportModel?[] importModels = await importBase.GetImportModelsAsync(importTasksDetails, uiSettings,
                            _protocolLogger,verify, cancellationToken);
                        contextLogger.Information("Загрузка пользователей завершена");

                        contextLogger.Information("Начало сохранения пользователей");
                        await analyzer.SaveAsync(importModels.ToList(), uiSettings, cancellationToken);
                        contextLogger.Information("Сохранение пользователей завершено");
                    }
                    
                }
                catch (Exception e)
                {
                    exception = e;
                }
                finally
                {
                    try
                    {
                        _protocolLogger.Information("Отправка сигнала о завершении");
                        await _importToScheduleServiceClientApi.StopTaskAsync(
                            new TaskCallbackRequest()
                            {
                                ID = importTasksDetails.ID, ErrorMessage = exception?.Message ?? String.Empty,
                                Result = exception == null
                            },
                            cancellationToken);
                        _protocolLogger.Information("Задача импорта завершена");
                    }
                    catch (Exception e)
                    {
                        finallyException = e;
                    }
                }

                var exceptions = new List<Exception>();

                if (exception != null)
                    exceptions.Add(exception);

                if (finallyException != null)
                    exceptions.Add(finallyException);

                if (exceptions.Any())
                {
                    foreach (var e in exceptions!)
                    {
                        _protocolLogger.Error(e,"Ошибка импорта");
                    }
                    //todo:сделать в валидации проверку наличия комнаты для создания рабочих ме
                    throw new AggregateException(exceptions);
                }
            }
            finally

            {
                _protocolLogger.CheckCreateValidProtocol();
                _protocolLogger.FlushAndClose();
            }
        }

        private async Task Verify(UISetting uiSettings, IProtocolLogger logger, CancellationToken cancellationToken)
        {
            var result = await _validationBLL.ValidateAsync(uiSettings.ID, null, cancellationToken);
            if (!result.Result)
            {
                var builder = new StringBuilder();
                builder.AppendLine("Задача не верифицирована.");
                builder.AppendLine("Ошибки верификации задачи:");
                foreach (var errror in result.CommonError)
                {
                    builder.AppendLine(errror);
                }

                builder.AppendLine("Ошибки верификации полей:");
                foreach (var fieldData in result.FieldData)
                {
                    builder.Append(fieldData.FieldName).AppendLine(":");
                    foreach (var error in fieldData.Errors)
                    {
                        builder.AppendLine(error);
                    }
                }

                var message = builder.ToString();
                logger.Information(message);
                throw new InvalidDataException(message);
            }
        }


        public async Task<ImportTasksDetails[]> GetImportTasksAsync(CancellationToken cancellationToken)
        {
            var tasks = await _readOnlyUISettingRepository.ToArrayAsync(cancellationToken);
            return _mapper.Map<ImportTasksDetails[]>(tasks);
        }

        public async Task<ImportMainTabDetails> GetMainTabAsync(Guid id, CancellationToken cancellationToken)
        {
            var setting = await _readOnlyUISettingRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

            var mainTabSettings = _mapper.Map<ImportMainTabDetails>(setting);

            var polledObjectDetails = _polledObjectConverter.GetPolledObjects(setting.ObjectType);
            mainTabSettings.PolledObject = JsonConvert.SerializeObject(polledObjectDetails);
            var uiSetting = await _importBaseMapper.Map(setting.ProviderType)
                .GetIDConfiguration(setting.ID, cancellationToken);
            mainTabSettings.SelectedConfiguration = uiSetting;

            return mainTabSettings;
        }


        public async Task<Guid> CreateMainTabAsync(ImportMainTabModel mainTabModel, CancellationToken cancellationToken)
        {
            var uiSetting = _mapper.Map<UISetting>(mainTabModel);
            var polledObject = JsonConvert.DeserializeObject<PolledObjectDetails>(mainTabModel.PolledObject);
            uiSetting.ObjectType = _polledObjectConverter.GetNumberFromPolledString(polledObject);

            _uiSettingsRepository.Insert(uiSetting);

            await _saveChangesCommand.SaveAsync(cancellationToken);

            await _importBaseMapper.Map(uiSetting.ProviderType).SetUISettingAsync(uiSetting.ID,
                mainTabModel.SelectedConfiguration, cancellationToken);

            return uiSetting.ID;
        }


        public async Task UpdateMainTabAsync(Guid id, ImportMainTabDetails mainTabDetails,
            CancellationToken cancellationToken)
        {
            var uiSetting = await _uiSettingsRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

            _mapper.Map(mainTabDetails, uiSetting);
            var polledObjectsString = mainTabDetails.PolledObject;
            var polledObjects = JsonConvert.DeserializeObject<PolledObjectDetails>(polledObjectsString);
            uiSetting.ObjectType = _polledObjectConverter.GetNumberFromPolledString(polledObjects);
            await _importBaseMapper.Map(uiSetting.ProviderType).UpdateUISettingAsync(uiSetting.ID,
                mainTabDetails.SelectedConfiguration, cancellationToken);

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<DeleteDetails> DeleteTaskAsync(Guid id, CancellationToken cancellationToken)
        {
            var resultDeleteFromScheduler =
                await _importToScheduleServiceClientApi.DeleteTaskAsync(id, cancellationToken);

            //TODO если не получилось удалить статус ответа != 200
            if (!resultDeleteFromScheduler)
            {
                return new DeleteDetails(true, Resources.CantDeleteTaskImport);
            }

            var setting = await _uiSettingsRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

            await _importBaseMapper.Map(setting.ProviderType).DeleteUISettingAsync(id, cancellationToken);
            _uiSettingsRepository.Delete(setting);

            await _saveChangesCommand.SaveAsync(cancellationToken);

            return new DeleteDetails();
        }

        public async Task<AdditionalTabDetails> GetAdditionalTabAsync(Guid id, CancellationToken cancellationToken)
        {
            var settings = await _readOnlyUISettingRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            var additionalTab = _mapper.Map<AdditionalTabDetails>(settings);
            await _analyzer.ModifyAdditionalTab(additionalTab, settings, cancellationToken);
            return additionalTab;
        }

        public async Task UpdateAdditionalTabAsync(Guid id, AdditionalTabData settings,
            CancellationToken cancellationToken)
        {
            var entity = await _uiSettingsRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
            _mapper.Map(settings, entity);

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }


        private long GetNumberFromPolledString<T>(string polledObjectJson)
        {
            var polledObjects = JsonConvert.DeserializeObject<T>(polledObjectJson);

            long polledObjectNumber = 0;
            var propertyInfos = polledObjects?.GetType().GetProperties() ?? Enumerable.Empty<PropertyInfo>();
            foreach (var item in propertyInfos)
            {
                if ((bool) (item.GetValue(polledObjects) ?? false))
                {
                    var number = (long) Enum.Parse(typeof(ObjectType), item.Name);
                    polledObjectNumber |= number;
                }
            }

            return polledObjectNumber;
        }


        private async Task<long> ChangeObjectTypeByIgnoredPropertyFromCongiguration(UISetting setting,
            CancellationToken cancellationToken)
        {
            var idConfiguration = await _importBaseMapper.Map(setting.ProviderType)
                .GetConfigurationIDBySettingAsync(setting.ID, cancellationToken);
            var fullObjectType = await GetPolledObjectsAsync(setting, idConfiguration, cancellationToken);
            return GetNumberFromPolledString<FullObjectTypeDetails>(fullObjectType);
        }

        private async Task<string> GetPolledObjectsAsync(UISetting setting, Guid? idConfiguration,
            CancellationToken token)
        {
            var polledObjectEnum = Enum.GetValues(typeof(ObjectType)).Cast<long>();
            var fullObjectType = new FullObjectTypeDetails();

            foreach (var item in polledObjectEnum)
            {
                var isIngoredPropertyInMainConfig =
                    await IsIgnoredPropertyInMainConfigAsync((ObjectType) item, idConfiguration, setting, token);
                if (isIngoredPropertyInMainConfig || (setting.ObjectType & item) > 0)
                {
                    var stringProperty = ((ObjectType) item).ToString();
                    PropertyInfo? property = fullObjectType.GetType().GetProperty(stringProperty);

                    if (property is not null)
                    {
                        property.SetValue(fullObjectType, true, null);
                    }
                }
            }

            return JsonConvert.SerializeObject(fullObjectType);
        }

        //Так как у нас не все проперти могут быть доступны из опрашиваемых обьектов, то мы выставляем их в опрашиваемые из конфигурации
        private async Task<bool> IsIgnoredPropertyInMainConfigAsync(ObjectType objectType, Guid? idConfiguration,
            UISetting setting, CancellationToken token)
        {
            if (_ignoredObjectTypes.Contains(objectType))
            {
                var importBase = _importBaseMapper.Map(setting.ProviderType);

                var fields = await importBase.GetFieldsByConfigurationIDAsync(idConfiguration, token);

                var listFields = fields.Select(x => new
                    {
                        Name = ((ObjectType) x.IMFieldID).ToString(),
                        Value = x.IMFieldID,
                        Field = x
                    })
                    .ToList();

                var resultField = listFields.FirstOrDefault(x => x.Name.Contains(objectType.ToString()));

                if (resultField is not null && !string.IsNullOrEmpty(resultField.Field.Expression))
                {
                    return true;
                }
            }

            return false;
        }
    }
}