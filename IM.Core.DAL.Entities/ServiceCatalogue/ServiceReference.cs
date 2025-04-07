using System;

namespace InfraManager.DAL.ServiceCatalogue
{
    public class ServiceReference
    {
        public ServiceReference()
        { }

        public ServiceReference(ObjectClass classID, Guid objectID, Guid serviceID)
        {
            ID = Guid.NewGuid();
            ClassID = classID;
            ObjectID = objectID;
            ServiceID = serviceID;
        }

        public Guid ID { get; init; }
        public ObjectClass ClassID { get; set; }
        public Guid ObjectID { get; set; }
        public Guid ServiceID { get; set; }
    }
}
