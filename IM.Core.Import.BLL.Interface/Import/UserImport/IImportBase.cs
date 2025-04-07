using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager.DAL.Import;
using InfraManager.ServiceBase.ScheduleService;

namespace IM.Core.Import.BLL.Interface.Import.OSU
{
    /// <summary>
    /// Интерфейс для сущности Импорт
    /// </summary>
    public interface IImportBase
    {
        /// <summary>
        /// Метод получает идентификатор конфигурации
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> идентификатор конфигурации </returns>
        Task<Guid?> GetIDConfiguration(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод удаляет конфигурацию
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task DeleteUISettingAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод задает конфигурацию
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="selectedConfiguration">идентификатор выбранной конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task SetUISettingAsync(Guid id, Guid? selectedConfiguration, CancellationToken cancellationToken);

        /// <summary>
        /// Метод модели импорта
        /// </summary>
        /// <param name="importDetails">модель задачи импорта</param>
        /// <param name="settings">настройки импорта</param>
        /// <param name="protocolLogger"></param>
        /// <param name="verify">Метод верификации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <param name="path">путь до файла импорта</param>
        /// <returns> массив моделей импорта </returns>
        Task<ImportModel?[]> GetImportModelsAsync(ImportTaskRequest importDetails, UISetting settings,
            IProtocolLogger protocolLogger,
            Func<UISetting?, IProtocolLogger, CancellationToken, Task> verify,
            CancellationToken cancellationToken);
        /// <summary>
        /// Метод обновляет конфигурацию
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="selectedConfiguration">идентификатор выбранной конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task UpdateUISettingAsync(Guid iD, Guid? selectedConfiguration, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получение идентификатора конфигурации по идентификатору настроек
        /// </summary>
        /// <param name="settingID">идентификатор настроек</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<Guid?> GetConfigurationIDBySettingAsync(Guid settingID, CancellationToken cancellationToken);

        /// <summary>
        /// Метод получения полей конфигурации по идентификатору конфигурации
        /// </summary>
        /// <param name="idConfiguration">идентификатор конфигурации</param>
        /// <param name="token"></param>
        Task<IEnumerable<UIIMFieldConcordance>> GetFieldsByConfigurationIDAsync(Guid? idConfiguration,
            CancellationToken token);
    }
}