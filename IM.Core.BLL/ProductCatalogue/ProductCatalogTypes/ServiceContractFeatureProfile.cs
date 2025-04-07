using AutoMapper;
using InfraManager.BLL.ProductCatalogue.ProductCatalogTypes.ServiceContractFeatures;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;
internal sealed class ServiceContractFeatureProfile : Profile
{
    public ServiceContractFeatureProfile()
    {
        CreateMap<ServiceContractFeature, ServiceContractFeatureDetails>();

        CreateMap<ServiceContractFeatureData, ServiceContractFeature>();
    }
}
