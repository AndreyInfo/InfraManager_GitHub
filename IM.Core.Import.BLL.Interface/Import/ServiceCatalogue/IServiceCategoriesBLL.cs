using InfraManager.DAL.ServiceCatalogue;

namespace IM.Core.Import.BLL.Interface.Import.ServiceCatalogue
{
    /// <summary>
    /// Интерфейс для сущности Категорий сервисов
    /// </summary>
    public interface IServiceCategoriesBLL
    {
        /// <summary>
        /// Метод создает категорию сервиса
        /// </summary>
        /// <param name="serviceCategory">категория сервиса</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task CreateAsync(ServiceCategory serviceCategory, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод получает категорию сервиса по идентификатору
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<ServiceCategory> GetServiceCategoryByIDAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает категорию сервиса по имени категории
        /// </summary>
        /// <param name="categoryName">имя категории</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<ServiceCategory?> GetServiceCategoryByNameAsync(string categoryName, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает категории сервисов по имени категории
        /// </summary>
        /// <param name="serviceCategoriesName">список имен категории</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<IEnumerable<ServiceCategory>> GetAllServiceCategoriesByNameAsync(List<string> serviceCategoriesName, CancellationToken cancellationToken);

    }
}
