using InfraManager.ServiceBase.ImportService.WebAPIModels.ServiceCatalogue;


namespace IM.Core.Import.BLL.Interface.Configurations
{
    /// <summary>
    /// Интерфейс для сущности Конфигурация каталога сервисов CSV
    /// </summary>
    public interface ISCConfigurationCSVBLL
    {
        /// <summary>
        /// Метод удаляет конфигурацию
        /// </summary>
        /// <param name="id">идентификатор конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        Task DeleteConfigurationAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Метод получает конфигурацию
        /// </summary>
        /// <param name="id">идентификатор конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> конфигурация </returns>
        Task<ServiceCatalogueImportCSVConfigurationDetails> GetConfigurationAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает все конфигурации
        /// </summary>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> конфигурации </returns>
        Task<ServiceCatalogueImportCSVConfigurationDetails[]> GetConfigurationsAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Метод задает конфигурацию
        /// </summary>
        /// <param name="configurationData">детали конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> идентификатор созданной конфигурации </returns>
        Task<Guid?> SetConfigurationAsync(ServiceCatalogueImportCSVConfigurationData configurationData, CancellationToken cancellationToken);
        /// <summary>
        /// Метод обновляет конфигурацию
        /// </summary>
        /// <param name="id">идентификатор конфигурации</param>
        /// <param name="configurationData">детали конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task UpdateConfigurationAsync(Guid id, ServiceCatalogueImportCSVConfigurationData configurationData, CancellationToken cancellationToken);
    }
}
