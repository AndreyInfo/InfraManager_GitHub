using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;

namespace IM.Core.Import.BLL.Interface.Import.ITAsset;

/// <summary>
/// Интерфейс для сущности получения моделей из CSV
/// </summary>
public interface IITAssetImportModelModificator
{
    /// <summary>
    /// Метод записсывает в БД массив ит-активов
    /// </summary>
    /// <param name="protocolLogger">логгер протоколов импорта</param>
    /// <param name="importTasksDetails">настройки запуска задачи из шедулера</param>
    /// <param name="settings">настройки задачи</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>Массив полей импорта</returns>
    Task<ITAssetImportDetails[]> GetModelsAsync(IProtocolLogger protocolLogger, ImportTasksDetails importTasksDetails, ITAssetImportSettingData? settings, CancellationToken cancellationToken);
}
