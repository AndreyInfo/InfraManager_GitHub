using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.UsersActivityType.Obsolete
{
    /// <summary>
    /// Интерфейс для работы со справочником видов деятельности
    /// </summary>
    [Obsolete("Use InfraManager.BLL.UsersActivityType.IUserActivityTypeBLL instead.")]
    public interface IUserActivityTypeBLL
    {
        /// <summary>
        /// Метод получает все записи о видах деятельности
        /// </summary>
        [Obsolete("Use InfraManager.BLL.UsersActivityType.IUserActivityTypeBLL.GetDetailsArray() instead.")]
        Task<UserActivityTypeDetails[]> ListAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Метод получает вид деятельности по Id родителя из базы данных 
        /// </summary>
        /// <param name="parentID">ParentID</param>
        [Obsolete("Use InfraManager.BLL.UsersActivityType.IUserActivityTypeBLL.GetDetailsArray() instead.")]
        Task<UserActivityTypeDetails[]> FindByParentAsync(Guid? parentID, CancellationToken cancellationToken);
        /// <summary>
        /// Метод удаляет вид деятельности из базы данных 
        /// </summary>
        /// <param name="id">ID</param>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод обновляет данные о виде деятельности в базе данных
        /// </summary>
        ///<param name="updateUserActivityTypeDetails"> модель создержащая id, name, parentID</param>
        Task<Guid> UpdateAsync(UserActivityTypeDetails userActivityTypeDetails, CancellationToken cancellationToken);
        /// <summary>
        /// Метод создает новый вид деятельности в базе данных
        /// </summary>
        /// <param name="updateUserActivityTypeDetails"> модель создержащая id, name, parentID</param>
        Task<Guid> CreateAsync(UserActivityTypeDetails userActivityTypeDetails, CancellationToken cancellationToken);
    }
}
