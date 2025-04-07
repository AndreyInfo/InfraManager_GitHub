using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    public class NotificationUserData
    {
        public Guid NotificationID { get; init; }
        public Guid UserID { get; init; }
        public int BusinessRole { get; init; }
    }
}
