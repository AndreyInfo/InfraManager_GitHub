using System;

namespace InfraManager.DAL.ProductCatalogue;
public class ServiceContractFeature
{
    public Guid ProductCatalogTypeID { get; init; }

    public ServiceContractFeatureEnum Feature { get; init; }

    public virtual ProductCatalogType ProductCatalogType { get; init; }
}
