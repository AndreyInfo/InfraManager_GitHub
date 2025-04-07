using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.AccessManagement;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfraManager.BLL.CrudWeb;
using InfraManager.DAL.AccessManagement;
using InfraManager.BLL.AccessManagement.AccessPermissions;

namespace InfraManager.BLL.AccessManagement
{
    /// <summary>
    /// Предназначен для работы с правами доступа
    /// разрешает той или иной сущности редактировать/добавлять/просматривать/удалять какой либо объект
    /// на данный момент заточен под логику для Портфеля Сервисов
    /// </summary>
    public interface IAccessPermissionBLL
    {
        /// <summary>
        /// Получение сущностей которым выданы права по объекту
        /// </summary>
        /// <param name="filter">элемент для скролинга и поиска, так же соержит в себе данные о сущнности к которой ищут права ID и ClassID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AccessPermissionDetails[]> GetDataTableAsync(BaseFilterWithClassIDAndID<Guid> filter, CancellationToken cancellationToken);
        
        /// <summary>
        /// получение прав по ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AccessPermission> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// добавление прав той или иной сущности
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Guid> AddAsync(AccessPermissionDetails model, CancellationToken cancellationToken = default);

        /// <summary>
        /// добавление прав той или иной сущности
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Guid> UpdateAsync(AccessPermissionDetails model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление права доступа к сущности
        /// </summary>
        /// <param name="accessPermissionID">идентификатор права</param>
        /// <param name="ownerID">сущность для которой удаляют права</param>
        /// <param name="cancellationToken"></param>
        Task RemoveAsync(Guid accessPermissionID, Guid ownerID, CancellationToken cancellationToken);


        /// <summary>
        /// Получение прав к объекту для теекущего пользователя
        /// Права складываются с правами групп, подразделений и организаций в которых состоит пользователь
        /// </summary>
        /// <param name="objectID">идентификатор объекта</param>
        /// <param name="objectClassID">тип объекта</param>
        /// <param name="cancellationToken"></param>
        /// <returns>права которые позволено совершать пользователю</returns>
        Task<AccessPermissionData> GetAccessUserToObjectByIDAsync(Guid objectID, ObjectClass objectClassID, CancellationToken cancellationToken);
    }
}
