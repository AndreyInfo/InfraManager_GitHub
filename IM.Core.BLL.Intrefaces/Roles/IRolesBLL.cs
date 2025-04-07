using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.AccessManagement.Roles;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.BLL.Roles
{
    /// <summary>
    /// бизнес логика для работы с сущностью Role
    /// </summary>
    public interface IRolesBLL
    {
        /// <summary>
        /// добавление сущности с связями operations и  lifeCycleOperations
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RoleDetails> AddAsync(RoleInsertDetails model, CancellationToken cancellationToken);

        /// <summary>
        /// обновление сущности с связями operations и  lifeCycleOperations, обновляет сами связи, а не объекты Operation и LifeCycleOperation
        /// </summary>
        /// <param name="id">Идентификатор Роли</param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RoleDetails> UpdateAsync(Guid id, RoleData model, CancellationToken cancellationToken);


        /// <summary>
        /// получение сущности с связями operations и  lifeCycleOperations
        /// </summary>
        /// <param name="id">идентификатор получаемой сущности</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RoleDetails> GetAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Удаление сущности по идентификатору
        /// </summary>
        /// <param name="id">идентификатор удаляемой сущности</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает список всех ролей + ставит галочки у ролей, которые есть у пользователя
        /// </summary>
        /// <param name="userID">идентификатор пользоватеоя</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Список ролей</returns>
        Task<UserRolesWithSelectedDetails[]> GetUserRolesAsync(Guid userID,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Перезаписывает роли пользователя
        /// </summary>
        /// <param name="userRoles">Выбранный список ролей</param>
        /// <param name="userID">идентификатор пользоватеоя</param>
        /// <param name="cancellationToken">токен отмены</param>
        Task SetRoleForUserAsync(UserRolesWithSelectedData[] userRoles, Guid userID,
            CancellationToken cancellationToken = default);

        /// Получение списка ролей для пользователя с деталями
        /// <param name="userId">Идентитфикатор пользователя, для которого получам роли</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns></returns>
        Task<RoleDetails[]> GetByUserAsync(Guid userID, CancellationToken cancellationToken);

        /// <summary>
        /// Получение списка всех ролей
        /// </summary>
        /// <param name="filterBy"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RoleDetails[]> GetDetailsArrayAsync(RoleFilter filterBy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение списка ролей по фильтру
        /// </summary>
        /// <param name="filterBy">Фильтр ролей</param>
        /// <param name="pageBy">Пагинация</param>
        /// <param name="cancellationToken">токен отмены</param>
        Task<RoleDetails[]> GetDetailsPageAsync(RoleFilter filterBy, ClientPageFilter<Role> pageBy,
            CancellationToken cancellationToken = default);
    }
}
