using InfraManager.DAL.Import.CSV;

namespace IM.Core.Import.BLL.Interface.Import.OSU
{
    /// <summary>
    /// Интерфейс для сущности Импорт CSV
    /// </summary>
    public interface IImportCSVBLL : IImportBase
    {
        /// <summary>
        /// Метод получает таблицу конфигурации CSV
        /// </summary>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> список конфигурации CSV </returns>
        Task<CSVConfigurationTable[]> GetConfigurationTableAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает путь до файла импорта
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> путь до папки с файлом </returns>
        Task<string> GetPathAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод обновляет путь до файла импорта
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="path">путь до папки с файлом задачи</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task UpdatePathAsync(Guid id, string path, CancellationToken cancellationToken);
    }
}
