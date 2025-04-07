using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.UsersActivityType.Obsolete
{
    /// <summary>
    /// Интерфейс для работы с добавлением связей видов деятельности к пользователям или группе
    /// </summary>
    [Obsolete("Use InfraManager.BLL.UsersActivityType.IUserActivityTypeBLL instead.")]
    public interface IUserActivityTypeReferenceBLL
    {
        /// <summary>
        /// Метод добавляет виды деятельности из списка к группе или пользователю
        /// </summary>
        /// <param name="models">модели, соединяющие виды деятельности и объекты принадлежности</param>
        Task<Guid[]> InsertAsync(UserActivityTypeReferenceDetails[] models, CancellationToken cancellationToken);
        /// <summary>
        /// Метод удаляет вид деятельности для определенной группы или пользователя
        /// </summary>
        /// <param name="ids"> идентефикаторы связи</param>
        Task DeleteAsync(Guid[] ids, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает все записи о видах деятельности для определенного id в формате без вложений
        /// </summary>
        /// <param name="objectId"> id объекта вставки пользователя или группы</param>
        [Obsolete("Use InfraManager.BLL.UsersActivityType.IUserActivityTypeBLL.GetDetailsArray() instead.")]
        Task<UserActivityTypeReferenceDetails[]> GetListByIdAsync(Guid objectId, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает все записи о видах деятельности для определенного id
        /// </summary>
        /// <param name="objectId"> id объекта вставки пользователя или группы</param>
        /// <param name="filter"> фильтр для таблицы</param>

        [Obsolete("Use InfraManager.BLL.UsersActivityType.IUserActivityTypeBLL.GetDetailsArray() instead.")]
        Task<UserActivityTypePathDetails[]> GetListUserActivityTypeByIdAsync(Guid objectId, BaseFilter filter, CancellationToken cancellationToken);
    }
}
