using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceCatalogue
{
    public class ServiceDetailsModel
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public Guid? CategoryID { get; init; }
        public Guid? CriticalityID { get; init; }
        public string CategoryUri { get; init; }
    }
}
