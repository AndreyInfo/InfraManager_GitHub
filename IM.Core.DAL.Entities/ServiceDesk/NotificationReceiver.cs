using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    public class NotificationReceiver
    {
        public NotificationReceiver(string eMail, ObjectClass? classID, bool? hasUserRole)
        {
            EMail = eMail;
            ClassID = classID;
            HasUserRole = hasUserRole;
        }
        public NotificationReceiver(string eMail)
        {
            EMail = eMail;
        }

        public string EMail { get; init; }
        public ObjectClass? ClassID { get; init; }
        public bool? HasUserRole { get; init; }
    }
}
