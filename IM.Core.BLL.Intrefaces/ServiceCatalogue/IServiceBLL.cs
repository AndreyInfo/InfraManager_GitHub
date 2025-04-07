using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue
{
    /// <summary>
    /// Этот инстерфейс описывает BLL сущности "Сервис"
    /// </summary>
    public interface IServiceBLL
    {
        /// <summary>
        /// Возвращает список сервисов, удовлетворяющих условиям выборки
        /// </summary>
        /// <param name="filterBy">Условия выборки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных сервисов</returns>
        public Task<ServiceDetails[]> GetDetailsPageAsync(ServiceListFilter filterBy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает данные сервиса
        /// </summary>
        /// <param name="id">Идентификатор сервиса</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Ссылка на объект с данными сервиса</returns>
        public Task<ServiceDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
