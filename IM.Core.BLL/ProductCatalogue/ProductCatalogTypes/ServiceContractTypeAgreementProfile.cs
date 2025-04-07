using AutoMapper;
using InfraManager.BLL.ProductCatalogue.ProductCatalogTypes.ServiceContractTypeAgreements;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;
internal sealed class ServiceContractTypeAgreementProfile : Profile
{
    public ServiceContractTypeAgreementProfile()
    {
        CreateMap<ServiceContractTypeAgreement, ServiceContractTypeAgreementDetails>()
            .ForMember(dst => dst.LifeCycleName, m => m.MapFrom(scr => scr.LifeCycle.Name));

        CreateMap<ServiceContractTypeAgreementData, ServiceContractTypeAgreement>()
            .ConstructUsing(c=> new(c.ProductCatalogTypeID, c.LifeCycleID));
    }
}
