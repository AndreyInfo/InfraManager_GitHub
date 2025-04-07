using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.WebApi.Contracts.Models.EMailProtocol
{
    /// <summary>
    /// Список адресов по запросу
    /// </summary>
    public class NotificationReceiverDetails
    {
        /// <summary>
        /// Адрес
        /// </summary>
        public string EMail { get; init; }
        /// <summary>
        /// Класс получателя уведомления, для которого указанный адрес
        /// </summary>
        public ObjectClass? ClassID { get; init; }
        /// <summary>
        /// Флаг наличия нужной роли у получателя
        /// </summary>
        public bool? HasUserRole { get; init; }
    }
}
