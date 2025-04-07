using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue
{
    /// <summary>
    /// Этот интерфейс описывает сервис бизнес-логики сущности "Категория сервиса"
    /// </summary>
    public interface IServiceCategoryBLL
    {
        /// <summary>
        /// Возвращает все категории сервисов, удовлетворяющих критерию
        /// </summary>
        /// <param name="filterBy">Критерии выборки данных</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных категорий сервисов</returns>
        Task<ServiceCategoryDetails[]> GetDetailsPageAsync(ServiceCategoryListFilter filterBy, CancellationToken cancellationToken = default);
        /// <summary>
        /// Возвращает данные категории сервиса по ключу
        /// </summary>
        /// <param name="id">Идентификатор категории сервиса</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Ссылка на данные категории сервиса</returns>
        Task<ServiceCategoryDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
