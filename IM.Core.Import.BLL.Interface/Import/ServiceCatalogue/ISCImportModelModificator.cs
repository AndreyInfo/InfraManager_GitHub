using InfraManager.ServiceBase.ImportService.WebAPIModels.ServiceCatalogue;
using InfraManager.ServiceBase.ScheduleService;

namespace IM.Core.Import.BLL.Interface.Import.ServiceCatalogue
{
    /// <summary>
    /// Интерфейс для сущности получения моделей из CSV
    /// </summary>
    public interface ISCImportModelModificator
    {
        /// <summary>
        /// Метод записсывает в БД массив категорий сервисов
        /// </summary>
        /// <param name="protocolLogger">логгер</param>
        /// <param name="importTasksDetails">настройки запуска задачи из шедулера</param>
        /// <param name="settings">настройки задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<SCImportDetail[]> GetModelsAsync(IProtocolLogger protocolLogger, ImportTaskRequest importTasksDetails, ServiceCatalogueImportSettingData? settings, CancellationToken cancellationToken);
    }
}
