using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.Asset.Filters;
using InfraManager.DAL.Asset;
using InfraManager.BLL.Location.Racks;

namespace InfraManager.BLL.Location
{
    public interface IRackBLL
    {
        /// <summary>
        /// Получение "шкафа" по <paramref name="id">целочисленному идентификатору</paramref>.
        /// </summary>
        /// <param name="id">целочисленный идентификатор</param>
        /// <returns>
        /// Объект типа <see cref="RackDetails"/> или null, если объекта с
        /// <paramref name="id">указанным ID</paramref> не существует.
        /// </returns>
        Task<RackDetails> DetailsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение коллекции "шкафов".
        /// </summary>
        /// <param name="listFilterBy">Фильтр для выборки.</param>
        /// <returns>
        /// <see cref="Array">Массив</see> объектов типа <see cref="RackDetails"/>.
        /// </returns>
        Task<RackDetails[]> GetDetailsArrayAsync(RackListFilter filterBy, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Получение списка шкафов, по фильтру
        /// </summary>
        /// <param name="filterBy">Условия выборки данных</param>\
        /// <param name="pageBy">Условия сортировки и постраничного вывода данных</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных шкафов, удовлетворяющих условию выборки</returns>
        Task<RackDetails[]> GetDetailsPageAsync(
            RackListFilter filterBy, ClientPageFilter<Rack> pageBy, CancellationToken cancellationToken);
    }
}
