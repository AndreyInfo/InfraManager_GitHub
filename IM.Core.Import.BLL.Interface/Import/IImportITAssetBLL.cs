using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;
using InfraManager.ServiceBase.ScheduleService;

namespace IM.Core.Import.BLL.Interface.Import;

/// <summary>
/// Интерфейс для сущности Импорта ит-активов
/// </summary>
public interface IImportITAssetBLL
{
    /// <summary>
    /// Метод запускает задачу импорта ит-активов
    /// </summary>
    /// <param name="importTasksDetails">модель задачи</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task StartImportAsync(ImportTasksDetails importTasksDetail, CancellationToken cancellationToken);

    /// <summary>
    /// Метод получает все задачи импорта ит-активов
    /// </summary>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>Массив параметров задания</returns>
    Task<ITAssetImportSettingDetails[]> GetAllImportTasksAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Метод получает задачу импорта ит-активов по идентификатору задачи
    /// </summary>
    /// <param name="id">идентификатор задачи</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>Параметры задания</returns>
    Task<ITAssetImportSettingDetails> GetImportTaskByIDAsync(Guid? id, CancellationToken cancellationToken);

    /// <summary>
    /// Метод создает задачу импорта ит-активов
    /// </summary>
    /// <param name="data">модель задачи</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task<Guid> CreateImportTaskAsync(ITAssetImportSettingData data, CancellationToken cancellationToken);

    /// <summary>
    /// Метод обновляет задачу импорта ит-активов
    /// </summary>
    /// <param name="id">идентификатор задачи</param>
    /// <param name="data">модель задачи</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task UpdateImportTaskAsync(Guid id, ITAssetImportSettingData data, CancellationToken cancellationToken);

    /// <summary>
    /// Метод удаляет задачу импорта ит-активов
    /// </summary>
    /// <param name="id">идентификатор задачи</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task DeleteImportTaskAsync(Guid id, CancellationToken cancellationToken);
}
