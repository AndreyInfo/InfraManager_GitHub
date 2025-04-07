using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;

namespace IM.Core.Import.BLL.Interface.Configurations;

/// <summary>
/// Интерфейс для сущности Конфигурация ит-активов CSV
/// </summary>
public interface IITAssetConfigurationCSVBLL
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
    Task<ITAssetImportCSVConfigurationDetails> GetConfigurationAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Метод получает все конфигурации
    /// </summary>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> конфигурации </returns>
    Task<ITAssetImportCSVConfigurationDetails[]> GetConfigurationsAsync(CancellationToken cancellationToken);
    /// <summary>
    /// Метод задает конфигурацию
    /// </summary>
    /// <param name="configurationData">детали конфигурации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> идентификатор созданной конфигурации </returns>
    Task<Guid> SetConfigurationAsync(ITAssetImportCSVConfigurationData configurationData, CancellationToken cancellationToken);
    /// <summary>
    /// Метод обновляет конфигурацию
    /// </summary>
    /// <param name="id">идентификатор конфигурации</param>
    /// <param name="configurationData">детали конфигурации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task UpdateConfigurationAsync(Guid id, ITAssetImportCSVConfigurationData configurationData, CancellationToken cancellationToken);

    /// <summary>
    /// Метод получает список типов из каталога продуктов
    /// </summary>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    Task<ProductCatalogTypeDetails[]> GetTypesAsync(CancellationToken cancellationToken);
}
