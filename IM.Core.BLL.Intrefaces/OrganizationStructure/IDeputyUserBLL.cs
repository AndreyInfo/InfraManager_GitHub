using Inframanager.BLL;
using InfraManager.DAL.OrganizationStructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.OrganizationStructure
{
    /// <summary>
    /// Бизнес логика для замещений
    /// </summary>
    public interface IDeputyUserBLL
    {
        /// <summary>
        /// Получение списка замещений удовлетворяющих условию выборки
        /// </summary>
        /// <param name="filterBy">Сссылка на объект с условиями выборки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Ссылка на массив данных замещений</returns>
        Task<DeputyUserDetails[]> GetDetailsArrayAsync(
            DeputyUserListFilter filterBy,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение списка моделей с возможностью поиска по пользователю которого замещают, а также фильтрацией завершенных замещений.
        /// </summary>
        /// <param name="filterBy"></param>
        /// <param name="pageFilter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DeputyUserDetails[]> GetDetailsPageAsync(
            DeputyUserListFilter filterBy,
            ClientPageFilter<DeputyUser> pageFilter,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Создание замещения
        /// </summary>
        /// <param name="data">данные создаваемого замещения</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DeputyUserDetails> AddAsync(DeputyUserData data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение данных замещения
        /// </summary>
        /// <param name="Id">идентификатор замещения</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DeputyUserDetails> DetailsAsync(Guid Id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Изменение существующей замещения
        /// </summary>
        /// <param name="Id">идентификатор существующей замещения</param>
        /// <param name="data">данные для обновления</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DeputyUserDetails> UpdateAsync(Guid Id, DeputyUserData data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление замещения
        /// </summary>
        /// <param name="Id">идентификатор замещения</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    }
}