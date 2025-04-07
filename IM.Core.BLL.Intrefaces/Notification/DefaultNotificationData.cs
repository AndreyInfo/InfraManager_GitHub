using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// оповещение по умолчанию
    /// </summary>
    public class DefaultNotificationData : NotificationData
    {
        public int DefaultBusinessRole { get; set; }
        public int AvailableBusinessRole { get; set; }
        public Dictionary<int, string> SelectedRoles { get; init; } = new Dictionary<int, string>();
    }
}
