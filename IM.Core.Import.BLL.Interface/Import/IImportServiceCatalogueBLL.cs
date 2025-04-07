using IM.Core.Import.BLL.Interface.Import.ServiceCatalogue;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ServiceCatalogue;
using InfraManager.ServiceBase.ScheduleService;

namespace IM.Core.Import.BLL.Interface.Import
{
    /// <summary>
    /// Интерфейс для сущности Импорта сервисов
    /// </summary>
    public interface IImportServiceCatalogueBLL
    {
        /// <summary>
        /// Метод получает все задачи импорта сервисов
        /// </summary>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<ServiceCatalogueImportSettingDetails[]> GetAllImportTasksAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает задачу импорта сервисов по идентификатору задачу
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<ServiceCatalogueImportSettingDetails> GetImportTaskByIDAsync(Guid? id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод создает задачу импорта сервисов
        /// </summary>
        /// <param name="data">модель задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<Guid> CreateImportTaskAsync(ServiceCatalogueImportSettingData data, CancellationToken cancellationToken);
        /// <summary>
        /// Метод обновляет задачу импорта сервисов
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="data">модель задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task UpdateImportTaskAsync(Guid id, ServiceCatalogueImportSettingData data, CancellationToken cancellationToken);
        /// <summary>
        /// Метод удаляет задачу импорта сервисов
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task DeleteImportTaskAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод запускает задачу импорта сервисов
        /// </summary>
        /// <param name="importTasksDetails">модель задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task StartImportAsync(ImportTaskRequest importTasksDetails, CancellationToken cancellationToken);

    }
}
