using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Notification
{
    public class NotificationRecipient
    {
        public Guid ID { get; init; }
        public Guid NotificationID { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
        public byte Scope { get; set; }

        public virtual Notification Notification { get; init; }
    }
}
