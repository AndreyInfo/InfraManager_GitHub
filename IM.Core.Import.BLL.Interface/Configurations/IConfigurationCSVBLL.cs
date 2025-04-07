using IM.Core.Import.BLL.Interface.Configurations.View;


namespace IM.Core.Import.BLL.Interface.Configurations
{
    /// <summary>
    /// Интерфейс для сущности Конфигурация CSV
    /// </summary>
    public interface IConfigurationCSVBLL
    {
        /// <summary>
        /// Метод удаляет конфигурацию
        /// </summary>
        /// <param name="id">идентификатор конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        Task DeleteConfigurationAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Метод получает конфигурацию CSV
        /// </summary>
        /// <param name="id">идентификатор конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> конфигурация </returns>
        Task<ConfigurationCSVDetails> GetConfigurationAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод задает конфигурацию CSV
        /// </summary>
        /// <param name="configurationCSVDetails">детали конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task SetConfigurationAsync(ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken);
        /// <summary>
        /// Метод обновляет конфигурацию CSV
        /// </summary>
        /// <param name="id">идентификатор конфигурации</param>
        /// <param name="configurationCSVDetails">детали конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task UpdateConfigurationAsync(Guid id, ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken);
    }
}
