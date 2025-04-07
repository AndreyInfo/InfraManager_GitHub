using InfraManager.DAL.ProductCatalogue;
using System;


namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes.ServiceContractFeatures;
public class ServiceContractFeatureData
{
    public Guid ProductCatalogTypeID { get; init; }

    public ServiceContractFeatureEnum Feature { get; init; }
}
