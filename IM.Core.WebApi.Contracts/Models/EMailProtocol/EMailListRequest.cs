using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.WebApi.Contracts.Models.EMailProtocol
{
    /// <summary>
    /// Запрос почтовых вдресов для получателся уведомления
    /// </summary>
    public class EMailListRequest
    {
        /// <summary>
        /// Инедтифткатор уведомления
        /// </summary>
        public Guid NotificationID { get; init; }
        
        /// <summary>
        /// Бизнес - роль получателя
        /// </summary>
        public int BusinessRole { get; init; }
        
        /// <summary>
        /// Сфера примения
        /// </summary>
        public int Scope { get; init; }
        
        /// <summary>
        /// Объект по которому уведомление
        /// </summary>
        public Guid ObjectID { get; init; }
    }
}
