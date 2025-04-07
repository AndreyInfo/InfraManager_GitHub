using InfraManager.DAL.Notification;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings.DefaultNotifications;

public interface ISupportSettingsNotificationsDefaultBLL
{
    /// <summary>
    /// Получение узлов дерева состоящее из классов
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы дерева оповещений по умолчанию</returns>
    Task<NodeNotificationDefaultDetails<ObjectClass>[]> GetClassNodesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение узлов дерева состоящее из бизнес ролей
    /// </summary>
    /// <param name="classID">идентификатор типа объекта</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы дерева из бизнес ролей</returns>
    Task<NodeNotificationDefaultDetails<BusinessRole>[]> GetBusinessRoleNodesAsync(ObjectClass classID, CancellationToken cancellationToken);

    /// <summary>
    /// Получение узлов дерева состоящее из оповещений
    /// </summary>
    /// <param name="classID">идентификатор типа объекта</param>
    /// <param name="businessRole">идетификатор бизнес роли</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы дерева из оповещений</returns>
    Task<NodeNotificationDefaultDetails<Guid>[]> GetNotificationNodesAsync(ObjectClass classID, BusinessRole businessRole, CancellationToken cancellationToken);

    /// <summary>
    /// Меняет дефолнтое значение бизнес роли для оповещения, в состояние выбрано
    /// </summary>
    /// <param name="notificationID">идентификатор опопвещния</param>
    /// <param name="cancellationToken"></param>
    Task ToDefaultBuisnessRoleAsync(Guid notificationID, BusinessRole businessRole, CancellationToken cancellationToken);


    /// <summary>
    /// Меняет дефолнтое значение бизнес роли для оповещения, в состояние не выбрано
    /// </summary>
    /// <param name="notificationID">идентификатор опопвещния</param>
    /// <param name="cancellationToken"></param>
    Task ToNoDefaultBuisnessRoleAsync(Guid notificationID, BusinessRole businessRole, CancellationToken cancellationToken);
}
