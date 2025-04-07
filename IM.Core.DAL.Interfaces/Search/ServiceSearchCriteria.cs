using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.DAL.Search
{
    public class ServiceSearchCriteria : SearchCriteria
    {
        public ServiceType[] Types { get; init; } = new[] { ServiceType.External };
        public CatalogItemState[] States { get; init; } =
            new[] { CatalogItemState.Worked, CatalogItemState.Blocked };
        public Guid[] ExceptServiceIDs { get; init; }
    }
}
