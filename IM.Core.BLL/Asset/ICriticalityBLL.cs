using Inframanager.BLL;
using InfraManager.DAL.Asset;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset
{
    /// <summary>
    /// Этот интерфейс описывает BLL сервис для сущности "Критичность"
    /// </summary>
    public interface ICriticalityBLL
    {
        /// <summary>
        /// Получает упорядоченный набор данных справочника "Критичность", разбитый на страницы
        /// </summary>
        /// <param name="filterCriteria">Критерии фильтрации</param>
        /// <param name="pageCriteria">Критерии сортировки и разбиения на страницы</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных справочника</returns>
        public Task<LookupDetails<Guid>[]> GetDetailsPageAsync(
            LookupListFilter filterCriteria,
            ClientPageFilter<Criticality> pageCriteria,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает данные одно элемента справочника "Критичность" по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные элемента справочника</returns>
        public Task<LookupDetails<Guid>> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавляет новый элемент в справочник "Критичность"
        /// </summary>
        /// <param name="data">Данные нового элемента</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные нового элемента после добавления</returns>
        public Task<LookupDetails<Guid>> AddAsync(LookupData data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Изменяет данные элемента справочника "Критичность"
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        /// <param name="data">Новый набор данных</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные элемента после изменения</returns>
        public Task<LookupDetails<Guid>> UpdateAsync(Guid id, LookupData data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет элемент из справочника "Критичность"
        /// </summary>
        /// <param name="id">Идентификатор элемента, который необходимо удалить</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
