using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Exceptions;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.ServiceCatalogue;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import.ServiceCatalogue;
using InfraManager.ResourcesArea;
using InfraManager.ServiceBase.ImportService;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ServiceCatalogue;
using InfraManager.ServiceBase.ScheduleService;
using Microsoft.Extensions.Logging;



namespace IM.Core.Import.BLL.Import
{
    public class ImportServiceCatalogueBLL : IImportServiceCatalogueBLL, ISelfRegisteredService<IImportServiceCatalogueBLL>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ImportServiceCatalogueBLL> _logger;
        private readonly IRepository<ServiceCatalogueImportSetting> _scImportSettingsRepository;
        private readonly IReadonlyRepository<ServiceCatalogueImportSetting> _scImportSettingsReadonlyRepository;
        private readonly ISCImportModelModificator _modelModificator;
        private readonly IScheduleServiceWebApi _importToScheduleServiceClientApi;
        private readonly IImportSCAnalyzerBLL _analyzer;

        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IProtocolLogger _protocolLogger;

        public ImportServiceCatalogueBLL(IMapper mapper,
            ILogger<ImportServiceCatalogueBLL> logger,
            IRepository<ServiceCatalogueImportSetting> scImportSettingsRepository,
            IReadonlyRepository<ServiceCatalogueImportSetting> scImportSettingsReadonlyRepository,
            IUnitOfWork saveChangesCommand,
            ISCImportModelModificator modelModificator,
            IScheduleServiceWebApi importToScheduleServiceClientApi,
            IImportSCAnalyzerBLL analyzer,
            IProtocolLogger protocolLogger)
        {
            _mapper = mapper;
            _logger = logger;
            _scImportSettingsRepository = scImportSettingsRepository;
            _scImportSettingsReadonlyRepository = scImportSettingsReadonlyRepository;
            _saveChangesCommand = saveChangesCommand;
            _modelModificator = modelModificator;
            _importToScheduleServiceClientApi = importToScheduleServiceClientApi;
            _analyzer = analyzer;
            _protocolLogger = protocolLogger;
        }

        public async Task<Guid> CreateImportTaskAsync(ServiceCatalogueImportSettingData data, CancellationToken cancellationToken)
        {
            var setting = _mapper.Map<ServiceCatalogueImportSetting>(data);
            _scImportSettingsRepository.Insert(setting);

            await _saveChangesCommand.SaveAsync(cancellationToken);


            return setting.ID;
        }

        public async Task DeleteImportTaskAsync(Guid id, CancellationToken cancellationToken)
        {
            var setting = await _scImportSettingsRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken) ?? throw new ObjectNotFoundException($"Не найдена конфигурация с ID {id}");

            _scImportSettingsRepository.Delete(setting);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<ServiceCatalogueImportSettingDetails[]> GetAllImportTasksAsync(CancellationToken cancellationToken)
        {
            ServiceCatalogueImportSetting[] tasks = null;
            try
            {
                tasks = await _scImportSettingsReadonlyRepository.ToArrayAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogInformation("ERR Ошибка чтения списка задач на импорт портфелей сервисов");
                throw;
            }
            return _mapper.Map<ServiceCatalogueImportSettingDetails[]>(tasks);
        }

        public async Task<ServiceCatalogueImportSettingDetails> GetImportTaskByIDAsync(Guid? id, CancellationToken cancellationToken)
        {
            var setting = await _scImportSettingsReadonlyRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken) ?? throw new ObjectNotFoundException($"Не найдена конфигурация с ID {id}");
            return _mapper.Map<ServiceCatalogueImportSettingDetails>(setting);
        }

        public async Task StartImportAsync(ImportTaskRequest importTasksDetails, CancellationToken cancellationToken)
        {
            Exception exception = null;
            Exception finallyException = null;
            try
            {
                var settings =
                    await GetImportTaskByIDAsync(importTasksDetails.SettingID, cancellationToken)
                    ?? throw new ObjectNotFoundException($"Не найден Service Catalogue Setting с ID = {importTasksDetails.SettingID}");

                _protocolLogger.StartTask(settings.Name, InfraManager.DAL.Import.ImportTaskTypeEnum.ServiceCatalogue, (Guid)importTasksDetails.SettingID, importTasksDetails.ID);
                _protocolLogger.Information($"Запущена задача импорта номер: {importTasksDetails.SettingID},\n название задачи: {settings.Name},\nо задаче {settings.Note}\n  номер задачи планировщика: {importTasksDetails.ID}");

                _protocolLogger.Information("Загрузка сервисов");
                SCImportDetail[] importModels = await _modelModificator.GetModelsAsync(_protocolLogger, importTasksDetails, _mapper.Map<ServiceCatalogueImportSettingData?>(settings), cancellationToken);
                _protocolLogger.Information("Загрузка сервисов завершена");

                _protocolLogger.Information("Начало сохранения сервисов");
                await _analyzer.SaveAsync(importModels, _protocolLogger, cancellationToken);
                _protocolLogger.Information("Сохранение сервисов завершено");
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                try
                {
                    _protocolLogger.Information("Отправка сигнала о завершении.");
                    await _importToScheduleServiceClientApi.StopTaskAsync(
                        new TaskCallbackRequest() { ID = importTasksDetails.ID, ErrorMessage = exception?.Message ?? String.Empty, Result = exception == null },
                        cancellationToken);
                    _protocolLogger.Information("Задача импорта завершена.");
                }
                catch (Exception e)
                {
                    finallyException = e;
                }
                finally
                {
                    _protocolLogger.CheckCreateValidProtocol();
                    _protocolLogger.FlushAndClose();
                }
            }

            var exceptions = new List<Exception>();

            if (exception is not null)
                exceptions.Add(exception);

            if (finallyException is not null)
                exceptions.Add(finallyException);

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }

        public async Task UpdateImportTaskAsync(Guid id, ServiceCatalogueImportSettingData data, CancellationToken cancellationToken)
        {
            var setting = await _scImportSettingsReadonlyRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken) ?? throw new ObjectNotFoundException($"Не найдена конфигурация с ID {id}");
            _mapper.Map(data, setting);

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }
    }
}
