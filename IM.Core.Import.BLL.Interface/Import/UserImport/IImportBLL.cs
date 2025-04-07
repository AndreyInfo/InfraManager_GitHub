using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager.DAL.Import;
using InfraManager.ServiceBase.ScheduleService;
using IM.Core.Import.BLL.Interface.Configurations.View;
using InfraManager.BLL.Import;

namespace IM.Core.Import.BLL.Interface.Import
{
    /// <summary>
    /// Интерфейс для работы с импортом независимо от источника
    /// </summary>
    public interface IImportBLL
    {
        /// <summary>
        /// Метод главную вкладку импорта
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        Task<ImportMainTabDetails> GetMainTabAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает все задачи импорта
        /// </summary>
        Task<ImportTasksDetails[]> GetImportTasksAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Метод создает главную вкладку импорта
        /// </summary>
        /// <param name="mainTabDetails">параметры главной вкладки</param>
        Task<Guid> CreateMainTabAsync(ImportMainTabModel mainTabDetails, CancellationToken cancellationToken);
        /// <summary>
        /// Метод обновляет главную вкладку импорта
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="mainTabDetails">данные главной вкладки для изменения</param>
        Task UpdateMainTabAsync(Guid id, ImportMainTabDetails mainTabDetails, CancellationToken cancellationToken);
        /// <summary>
        /// Метод удаляет задачу импорта целиком
        /// </summary>
        /// <param name="id">идентификатор задачи импорта</param>
        Task<DeleteDetails> DeleteTaskAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает вкладку "Дополнительно" задачи импорта
        /// </summary>
        /// <param name="id">идентификатор задачи импорта</param>
        Task<AdditionalTabDetails> GetAdditionalTabAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод обновляет вкладку "Дополнительно" импорта
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="mainTabDetails">данные дополнительной вкладки для изменения</param>
        Task UpdateAdditionalTabAsync(Guid id, AdditionalTabData settings, CancellationToken cancellationToken);

        /// <summary>
        /// Метод запускает задачу импорта
        /// </summary>
        /// <param name="importTasksDetails">задача импорта</param>
        Task StartImportAsync(ImportTaskRequest importTasksDetails);
    }
}