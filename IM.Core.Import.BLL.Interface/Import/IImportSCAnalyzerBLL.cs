using IM.Core.Import.BLL.Interface.Import.ServiceCatalogue;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ServiceCatalogue;


namespace IM.Core.Import.BLL.Interface.Import
{
    /// <summary>
    /// Интерфейс для сущности запуска импорта портфелей сервисов
    /// </summary>
    public interface IImportSCAnalyzerBLL
    {
        /// <summary>
        /// Метод запускает процесс импорта
        /// </summary>
        /// <param name="importModels">модели для импорта</param>
        /// <param name="protocolLogger">логгер</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task SaveAsync(SCImportDetail[] importModels, IProtocolLogger protocolLogger, CancellationToken cancellationToken);
    }
}
