using System;

namespace InfraManager.BLL.ServiceCatalogue
{
    public class ServiceDetails
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public Guid? CategoryID { get; init; }
        public Guid? CriticalityID { get; init; }
    }
}
