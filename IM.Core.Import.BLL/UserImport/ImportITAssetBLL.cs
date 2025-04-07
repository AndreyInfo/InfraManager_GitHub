using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.ITAsset;
using InfraManager;
using InfraManager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Import.ITAsset;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;
using InfraManager.ServiceBase.ScheduleService;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL.Import;
public class ImportITAssetBLL : IImportITAssetBLL, ISelfRegisteredService<IImportITAssetBLL>
{
    private readonly IMapper _mapper;
    private readonly ILogger<ImportITAssetBLL> _logger;
    private readonly IRepository<ITAssetImportSetting> _itAssetImportSettingsRepository;
    private readonly IReadonlyRepository<ITAssetImportSetting> _itAssetImportSettingsReadonlyRepository;
    private readonly IScheduleServiceWebApi _importToScheduleServiceClientApi;
    private readonly IITAssetImportModelModificator _modelModificator;
    private readonly IImportITAssetAnalyzerBLL _analyzer;
    private readonly IUnitOfWork _saveChangesCommand;
    private readonly IProtocolLogger _protocolLogger;
    private readonly IPagingQueryCreator _paging;

    public ImportITAssetBLL(IMapper mapper,
        ILogger<ImportITAssetBLL> logger,
        IRepository<ITAssetImportSetting> itAssetImportSettingsRepository,
        IReadonlyRepository<ITAssetImportSetting> itAssetImportSettingsReadonlyRepository,
        IScheduleServiceWebApi importToScheduleServiceClientApi,
        IProtocolLogger protocolLogger,
        IITAssetImportModelModificator modelModificator,
        IImportITAssetAnalyzerBLL analyzer,
        IUnitOfWork saveChangesCommand,
        IPagingQueryCreator paging
        )
    {
        _mapper = mapper;
        _logger = logger;
        _itAssetImportSettingsRepository = itAssetImportSettingsRepository;
        _itAssetImportSettingsReadonlyRepository = itAssetImportSettingsReadonlyRepository;
        _importToScheduleServiceClientApi = importToScheduleServiceClientApi;
        _modelModificator = modelModificator;
        _saveChangesCommand = saveChangesCommand;
        _protocolLogger = protocolLogger;
        _analyzer = analyzer;
        _paging = paging;
    }

    public async Task StartImportAsync(ImportTasksDetails importTasksDetails, CancellationToken cancellationToken)
    {
        Exception exception = null;
        Exception finallyException = null;
        try
        {
            var settings = await GetImportTaskByIDAsync(importTasksDetails.ID, cancellationToken)
                ?? throw new ObjectNotFoundException($"Не найден IT Asset Setting с ID = {importTasksDetails.ID}");

            _protocolLogger.StartTask(settings.Name, InfraManager.DAL.Import.ImportTaskTypeEnum.ITAssets, (Guid)importTasksDetails.ID, importTasksDetails.ID);
            _protocolLogger.Information($"Запущена задача импорта номер: {importTasksDetails.ID},\n название задачи: {settings.Name},\nо задаче {settings.Note}\n  номер задачи планировщика: {importTasksDetails.ID}");


            _protocolLogger.Information("Загрузка ит-активов");
            ITAssetImportDetails[] importModels = await _modelModificator
                .GetModelsAsync(_protocolLogger, importTasksDetails, _mapper.Map<ITAssetImportSettingData?>(settings), cancellationToken);
            _protocolLogger.Information("Загрузка ит-активов завершена");

            _protocolLogger.Information("Начало сохранения ит-активов");
            await _analyzer.SaveAsync(settings, importModels, _protocolLogger, cancellationToken);
            _protocolLogger.Information("Сохранение ит-активов завершено");
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

    public async Task<Guid> CreateImportTaskAsync(ITAssetImportSettingData data, CancellationToken cancellationToken)
    {
        var setting = _mapper.Map<ITAssetImportSetting>(data);
        _itAssetImportSettingsRepository.Insert(setting);

        await _saveChangesCommand.SaveAsync(cancellationToken);

        return setting.ID;
    }

    public async Task DeleteImportTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        var setting = await _itAssetImportSettingsRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException($"Не найдена конфигурация с ID {id}");

        _itAssetImportSettingsRepository.Delete(setting);
        await _saveChangesCommand.SaveAsync(cancellationToken);
    }

    public async Task<ITAssetImportSettingDetails[]> GetAllImportTasksAsync(CancellationToken cancellationToken)
    {
        ITAssetImportSetting[] result;
        try
        {
            var query = _itAssetImportSettingsReadonlyRepository.Query();

            var paggingQuery = _paging.Create(query.OrderBy(x => x.Name));

            result = await paggingQuery.PageAsync(0, 0, cancellationToken);
        }
        catch
        {
            _logger.LogInformation("ERR Ошибка чтения списка задач на импорт ит-активов");
            throw;
        }

        return _mapper.Map<ITAssetImportSettingDetails[]>(result);
    }

    public async Task<ITAssetImportSettingDetails> GetImportTaskByIDAsync(Guid? id, CancellationToken cancellationToken)
    {
        var setting = await _itAssetImportSettingsReadonlyRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException($"Не найдена конфигурация с ID {id}");
        return _mapper.Map<ITAssetImportSettingDetails>(setting);
    }

    public async Task UpdateImportTaskAsync(Guid id, ITAssetImportSettingData data, CancellationToken cancellationToken)
    {
        var setting = await _itAssetImportSettingsReadonlyRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException($"Не найдена конфигурация с ID {id}");
        _mapper.Map(data, setting);

        await _saveChangesCommand.SaveAsync(cancellationToken);
    }
}
