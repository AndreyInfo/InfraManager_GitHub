using InfraManager.DAL.ServiceCatalogue;


namespace IM.Core.Import.BLL.Interface.Import.ServiceCatalogue
{
    /// <summary>
    /// Интерфейс для Сервиса
    /// </summary>
    public interface IServicesBLL
    {
        /// <summary>
        /// Метод создает сервисы
        /// </summary>
        /// <param name="services">сервисы для создания</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task CreateAsync(Service[] services, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод обновляет сервисы
        /// </summary>
        /// <param name="updateServices">модели для обновления</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task UpdateAsync(Dictionary<ServiceData, Service> updateServices, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод получает сервисы по внешнему идентификатору
        /// </summary>
        /// <param name="services">модели импорта сервисов</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<Service[]> GetAllServicesByExternalIDAsync(List<SCImportDetail> services, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает сервисы по имени и имени категории при пустом внешнем идентификаторе в БД
        /// </summary>
        /// <param name="services">модели импорта сервисов</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<Service[]> GetAllServicesByNameAndCategoryAndEmptyIDAsync(List<SCImportDetail> services, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает сервисы по имени и имени категории при заполненном внешнем идентификаторе в БД
        /// </summary>
        /// <param name="services">модели импорта сервисов</param>
        /// <param name="cancellationToken">отмена задачи</param>
        Task<Service[]> GetAllServicesByNameAndCategoryWithExternalIDAsync(List<SCImportDetail> services, CancellationToken cancellationToken);


    }
}
