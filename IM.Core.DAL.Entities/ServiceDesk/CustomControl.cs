using Inframanager;
using System;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk
{
    public class CustomControl
    {
        protected CustomControl()
        {
        }

        public CustomControl(Guid userID, InframanagerObject inframanagerObject)
        {
            UserId = userID;
            ObjectId = inframanagerObject.Id;
            ObjectClass = inframanagerObject.ClassId;
        }

        public Guid UserId { get; private set; }
        public Guid ObjectId { get; private set; }
        public ObjectClass ObjectClass { get; private set; }
    }
}
