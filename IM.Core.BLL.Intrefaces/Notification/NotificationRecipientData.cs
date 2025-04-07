using InfraManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// Получатель оповещения
    /// </summary>
    public class NotificationRecipientData
    {
        public Guid ID { get; init; }
        public Guid NotificationID { get; init; }
        public string Name { get; set; }
        public int BusinessRoleID { get; set; }
        public RecipientType Type { get; init; }
        public RecipientScope Scope { get; init; }
    }
}
