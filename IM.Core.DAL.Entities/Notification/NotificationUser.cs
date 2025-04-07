using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Notification
{
    public class NotificationUser
    {
        public Guid NotificationID { get; init; }
        public Guid UserID { get; set; }
        public int BusinessRole { get; set; }

        public virtual Notification Notification { get; init; }
    }
}
