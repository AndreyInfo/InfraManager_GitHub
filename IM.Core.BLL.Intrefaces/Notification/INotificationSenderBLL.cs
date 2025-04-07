using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    public interface INotificationSenderBLL
    {
        /// <summary>
        /// Отправляет указанное уведомление для указанного объекта
        /// </summary>
        /// <param name="setting">Тип настройки, храняшей идентификатор уведомления</param>
        /// <param name="object">Объект, по которому отправляется уведомление</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Флаг усепшной отправки уведомления</returns>
        Task<bool> SendNotificationAsync(SystemSettings setting, InframanagerObject @object, CancellationToken cancellationToken);

        /// <summary>
        /// Отправляет отдельные уведомления для указанного объекта
        /// </summary>
        /// <param name="setting">Тип настройки, храняшей идентификатор уведомления</param>
        /// <param name="object">Объект, по которому отправляется уведомление</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Флаг усепшной отправки уведомления</returns>
        Task<bool> SendSeparateNotificationsAsync(SystemSettings setting, InframanagerObject @object, CancellationToken cancellationToken);
    }
}
