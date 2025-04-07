using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager.DAL.Import;

namespace IM.Core.Import.BLL.Interface.Import
{
    /// <summary>
    /// Интерфейс для анализа действии при сохранении
    /// </summary>
    public interface IImportAnalyzerBLL
    {
        /// <summary>
        /// Метод производит выполнение анализа импорта и сохранение или обновление моделей
        /// </summary>
        /// <param name="importModels">модели импорта для анализа</param>
        /// <param name="uiSettings">настройки</param>
        /// <param name="protocolLogger">протокол импорта</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task SaveAsync(List<ImportModel?> importModels, UISetting uiSettings, CancellationToken cancellationToken);
        /// <summary>
        /// Метод производит модификацию модели вкладки Дополнительно с учетом наличия в системе разных параметров(Организации, Подразделений)
        /// </summary>
        /// <param name="additionalTab">модель вкладки Дополнительно</param>
        /// <param name="settings">настройки</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task ModifyAdditionalTab(AdditionalTabDetails additionalTab, UISetting settings, CancellationToken cancellationToken);
    }
}
