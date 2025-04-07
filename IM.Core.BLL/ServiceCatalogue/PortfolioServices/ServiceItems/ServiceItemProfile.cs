using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;

internal sealed class ServiceItemProfile : Profile
{
    public ServiceItemProfile()
    {

        CreateMap<ServiceItem, ServiceItemDetails>()
            .ForMember(
                c => c.ClassID,
                m => m.MapFrom(scr => ObjectClass.ServiceItem))
            .ForMember(
                c => c.StateName,
                m => m
                    .MapFrom<LocalizedEnumResolver<ServiceItem, ServiceItemDetails, CatalogItemState>,
                        CatalogItemState>(
                        entity => entity.State ?? entity.Service.State))
            .ForMember(
                dst => dst.CategoryID,
                m => m.MapFrom(src => src.Service.CategoryID))
            .ForMember(
                dst => dst.State,
                m => m.MapFrom(src => src.State ?? src.Service.State));


        CreateMap<ServiceItemData, ServiceItem>()
            .ConstructUsing(c => new ServiceItem(c.Name))
            .ForMember(
                dst => dst.Parameter,
                m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Parameter) ? "" : scr.Parameter))
            .ForMember(
                dst => dst.Note,
                m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Note) ? "" : scr.Note));


        CreateMap<ServiceItem, ServiceItemDetailsModel>()
               .ForMember(
                   model => model.ClassID,
                   mapper => mapper.MapFrom(entity => ObjectClass.ServiceItem))
               .ForMember(
                   model => model.ServiceCategoryID,
                   mapper => mapper.MapFrom(entity => entity.Service.Category.ID))
               .ForMember(
                   model => model.ServiceCategoryName,
                   mapper => mapper.MapFrom(entity => entity.Service.Category.Name))
               .ForMember(
                   model => model.ServiceID,
                   mapper => mapper.MapFrom(entity => entity.Service.ID))
               .ForMember(
                   model => model.ServiceName,
                   mapper => mapper.MapFrom(entity => entity.Service.Name))
               .ForMember(
                   model => model.FullName,
                   mapper => mapper.MapFrom(entity => $"{entity.Service.Category.Name} \\ {entity.Service.Name} \\ {entity.Name}"))
               .ForMember(
                   model => model.Summary,
                   mapper => mapper.MapFrom(entity => string.Empty));
    }
}
