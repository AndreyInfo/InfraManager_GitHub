using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// Тип уровня в дереве оповещений
    /// </summary>
    public enum NotificationTreeLevel
    {
        /// <summary>
        /// Объект
        /// </summary>
        Object = 1,
        /// <summary>
        /// Роль
        /// </summary>
        Role = 2,
        /// <summary>
        /// Оповещение
        /// </summary>
        Notification = 3
    }
}
