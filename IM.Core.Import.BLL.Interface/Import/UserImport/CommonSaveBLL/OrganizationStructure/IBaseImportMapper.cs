using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager.DAL;
using InfraManager;
using InfraManager.DAL.Import;

namespace IM.Core.Import.BLL.Interface
{
    /// <summary>
    /// Осуществляет обновление и создание объектов импорта
    /// </summary>
    public interface IBaseImportMapper<TDetails,TEntity>
    {
        /// <summary>
        /// Создает и инициализирует данные для обновления из различных источников
        /// </summary>
        /// <param name="models">Данные обновления пришедшие из источника</param>
        /// <param name="setting">Настройки импорта</param>
        /// <param name="token"></param>
        /// <returns>Данные, используемые в различных функциях обновления</returns>
        Task<ImportData<TDetails, TEntity>> Init(IEnumerable<ImportModel> models, UISetting setting,
            CancellationToken token);

        /// <summary>
        /// Создает и инициализирует выбранные флагами <param name="flags">flags</param> поля пользователя
        /// </summary>
        /// <param name="data">Данные о конфигурации импорта</param>
        /// <param name="details">Данные для обновления</param>
        /// <returns>Пользователь инициализированными полями, выбранными флагами</returns>
        IEnumerable<TEntity> Map(ImportData<TDetails, TEntity> data, IEnumerable<TDetails> details);

        /// <summary>
        /// Обновляет выбранные флагами поля пользователя значениями соответствующих полей деталей
        /// </summary>
        /// <param name="data">Данные о конфигурации импорта</param>
        /// <param name="updatePairs">Классы данных, из которых будут браться значения,
        ///     и классы пользователей, которые будут обновляться соответствующими данными</param>
        void Map(ImportData<TDetails, TEntity> data, IEnumerable<(TDetails, TEntity)> updatePairs);
    }
}
