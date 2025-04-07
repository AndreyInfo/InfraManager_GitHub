using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceCatalog;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

internal sealed class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Service, ServiceDetailsModel>()
            .ForMember(
                model => model.ServiceCategoryID,
                mapper => mapper.MapFrom(entity => entity.Category.ID));

        CreateMap<ServiceModelItem, ServiceDetails>()
            .ForMember(c => c.StateName, m => m.MapFrom<LocalizedEnumResolver<ServiceModelItem, ServiceDetails, CatalogItemState>, CatalogItemState>(
                        entity => entity.State.Value))
            .ForMember(c => c.TypeName, m => m.MapFrom<LocalizedEnumResolver<ServiceModelItem, ServiceDetails, ServiceType>, ServiceType>(
                        entity => entity.Type));


        CreateMap<Service, ServiceDetails>()
            .ForMember(model => model.ServiceCategoryID, mapper => mapper.MapFrom(entity => entity.CategoryID))
            .ForMember(c => c.ClassID, m => m.MapFrom(scr => ObjectClass.Service))
            .ForMember(c => c.ServiceCategoryName, m => m.MapFrom(c => c.Category.Name))
            .ForMember(c => c.StateName, m => m.MapFrom<LocalizedEnumResolver<Service, ServiceDetails, CatalogItemState>, CatalogItemState>(
                        entity => entity.State))
            .ForMember(c => c.TypeName, m => m.MapFrom<LocalizedEnumResolver<Service, ServiceDetails, ServiceType>, ServiceType>(
                        entity => entity.Type));

        CreateMap<ServiceData, Service>()
           .ConstructUsing(c => new(c.Name, c.Type, c.State.Value, c.ExternalID, c.ServiceCategoryID))
           .ForMember(model => model.CategoryID, mapper => mapper.MapFrom(entity => entity.ServiceCategoryID));
    }
}
