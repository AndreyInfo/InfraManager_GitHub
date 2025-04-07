using IM.Core.Import.BLL.Interface.Import.ITAsset;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;

namespace IM.Core.Import.BLL.Interface.Import;

/// <summary>
/// Интерфейс для сущности запуска импорта ит-активов
/// </summary>
public interface IImportITAssetAnalyzerBLL
{
    /// <summary>
    /// Метод запускает процесс импорта
    /// </summary>
    /// <param name="settings">настройки задания для импорта</param>
    /// <param name="importModels">модели для импорта</param>
    /// <param name="protocolLogger">логгер протоколов импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task SaveAsync(ITAssetImportSettingDetails settings, ITAssetImportDetails[] importModels, IProtocolLogger protocolLogger, CancellationToken cancellationToken);
}
