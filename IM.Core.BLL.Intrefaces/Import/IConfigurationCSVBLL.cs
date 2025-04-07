using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Import
{
    public interface IConfigurationCSVBLL
    {
        /// <summary>
        /// Возвращает конфигурацию
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ConfigurationCSVDetails> GetConfigurationAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Создает конфигурацию
        /// </summary>
        /// <param name="configurationCSVDetails">конфигурация</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SetConfigurationAsync(ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken);
        /// <summary>
        /// Обновляет конфигурацию
        /// </summary>
        /// <param name="id"></param>
        /// <param name="configurationCSVDetails">конфигурация</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateConfigurationAsync(Guid id, ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken);

        /// <summary>
        /// Метод удаляет конфигурацию
        /// </summary>
        /// <param name="id">идентификатор конфигурации</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        Task DeleteConfigurationAsync(Guid id, CancellationToken cancellationToken);
    }
}
