using InfraManager.BLL.Settings;
using InfraManager.DAL.Events;
using InfraManager.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// Оповещения
    /// </summary>
    public interface INotificationBLL
    {
        /// <summary>
        /// Возвращает оповещение
        /// </summary>
        /// <param name="id">Идентификатор оповещения</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NotificationDetails> FindAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает список оповещений
        /// </summary>
        /// <param name="filter">Фильтр сортировки</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NotificationData[]>
            GetNotificationsAsync(BaseFilter filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвоащает список доступных классов
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<InframanagerObjectClassData[]> GetClassesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает список доступных ролей для класса
        /// </summary>
        /// <param name="classID">Класс объекта</param>
        /// <returns></returns>
        public Dictionary<int, string> GetRolesByClass(ObjectClass classID);

        /// <summary>
        /// Добавляет оповещение
        /// </summary>
        /// <param name="notificationData">Оповещение</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Guid> AddNotificationAsync(NotificationDetails notificationData, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновляет оповещение
        /// </summary>
        /// <param name="notificationData">Оповещение</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Guid> UpdateNotificationAsync(NotificationDetails notificationData, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет оповещение
        /// </summary>
        /// <param name="id">Идентификатор оповещения</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteNotificationAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает список параметров для конкретного класса оповещения
        /// </summary>
        /// <param name="objectClass">Класс объекта</param>
        /// <returns></returns>
        ParameterTemplate[] GetAvailableParameterList(ObjectClass objectClass);

        /// <summary>
        /// Возвращает указанный уровень дерева оповещений по умолчанию
        /// </summary>
        /// <param name="level">уровень дерева</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DefaultNotificationTreeItem[]> GetDefaultNotificationTreeAsync(NotificationTreeLevel level, int parentId, ObjectClass? classId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохраняет оповещения по умолчанию
        /// </summary>
        /// <param name="notificationSaveDataList">список оповещений для обновления</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SaveDefaultNotificationsAsync(List<NotificationSaveData> notificationSaveDataList, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает указанный уровень дерева оповещений для указаного пользователя
        /// </summary>
        /// <param name="userID">идентификатор пользователя</param>
        /// <param name="level">уровень дерева</param>
        /// <param name="parentID">идентификатор родительсткого элемента</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DefaultNotificationTreeItem[]> GetUserNotificationTreeAsync(Guid userID, NotificationTreeLevel level, int parentID, ObjectClass? classID, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохраняет оповещения указанного пользователя
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="notificationSaveDataList">список оповещений для обновления</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SaveNotificationUserAsync(Guid userId, List<NotificationSaveData> notificationSaveDataList, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Синхронный метод получения имени уведомления по ID уведомления
        /// </summary>
        /// <param name="notificationID">ID  уведомления</param>
        string GetNameByID(Guid notificationID);

        /// <summary>
        /// Групповое сохранение оповешений
        /// </summary>
        /// <param name="userID">идентификатор пользователя</param>
        /// <param name="classID">идентификатор класса</param>
        /// <param name="level">уровень в дереве</param>
        /// <param name="roleID">идентификатор роли</param>
        /// <param name="isChecked">состояние</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task BulkSaveUserNotificationsAsync(Guid userID, ObjectClass classID, int? roleID, bool isChecked, CancellationToken cancellationToken = default);
    }
}
