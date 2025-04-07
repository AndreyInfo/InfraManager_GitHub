using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.Models
{
    /// <summary>
    /// Предоставляет единую точку доступа для всех моделей каталога продуктов
    /// </summary>
    public interface IProductCatalogModelBLLFacade
    {
        /// <summary>
        /// Возвращает список моделей продуктов для указанного типа продуктов
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список моделей для страницы для моделей с указанным типом.</returns>
        Task<ProductModelOutputDetails[]> GetModelsAsync(ProductCatalogModelFilter filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение данных модели по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор модели</param>
        /// <param name="modelClassID">Класс модели.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Данные модели</returns>
        Task<ProductModelOutputDetails> GetAsync(Guid id, ProductTemplate modelClassID,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Вставка модели 
        /// </summary>
        /// <param name="data">Данные модели</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Записанное</returns>
        Task<ProductModelOutputDetails> InsertAsync(ProductCatalogModelData data,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновление модели
        /// </summary>
        /// <param name="id">Идентификатор модели</param>
        /// <param name="template">Класс модели.</param>
        /// <param name="data">Данные модели</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Записанное</returns>
        Task<ProductModelOutputDetails> UpdateAsync(Guid id, ProductTemplate template, ProductCatalogModelData data,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление модели продукта
        /// </summary>
        /// <param name="productClass">Класс модели продукта</param>
        /// <param name="id">Идентификатор модели продукта</param>
        /// <param name="flags">Флаги удаления моделей</param>
        /// <param name="token">Токен отмены</param>
        Task DeleteAsync(ProductTemplate productClass, Guid id, ProductCatalogDeleteFlags flags, CancellationToken token = default);


        Task DeleteModelsByFilterAsync(ProductCatalogModelDeleteFilter filter
            , bool deleteWithObject
            , CancellationToken cancellationToken);

        Task<bool> IsUseTypeAsync(ProductCatalogModelDeleteFilter filter
            , CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает список моделей продуктов без проверки прав пользователя.
        /// </summary>
        /// <param name="filter">Фильтр по типу/категории.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список моделей.</returns>
        Task<ProductModelOutputDetails[]> GetModelsWithoutTTZAsync(ProductCatalogModelFilter filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение данных модели по идентификатору без проверки прав пользователя.
        /// </summary>
        /// <param name="id">Идентификатор модели.</param>
        /// <param name="modelClassID">Класс модели.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Данные модели.</returns>
        Task<ProductModelOutputDetails> GetWithoutTTZAsync(Guid id, ProductTemplate modelClassID,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновление модели без проверки прав пользователя.
        /// </summary>
        /// <param name="id">Идентификатор модели</param>
        /// <param name="modelClassID">Класс модели.</param>
        /// <param name="data">Данные модели</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Данные обновленной модели.</returns>
        Task<ProductModelOutputDetails> UpdateWithoutTTZAsync(Guid id, ProductTemplate modelClassID, ProductCatalogModelData data,
            CancellationToken cancellationToken = default);
    }
}