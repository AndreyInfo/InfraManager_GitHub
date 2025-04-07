using System;

namespace InfraManager.DAL.ServiceCatalogue
{
    public class ServiceTag
    {
        public Guid ObjectId { get; set; }

        public ObjectClass ClassId { get; set; }

        public string Tag { get; set; }
    }
}
